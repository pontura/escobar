using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerManager : MonoBehaviour {
    public VideoPlayer videoPlayer;
    public GameObject loading;

    public float videoCheckRate;
    public float maxPrepareTime;
    public float prepareTimeStep;

    public float prepareTime;

    public double duration;

    public double lastTime;

    bool playing;

    PlaylistData.Sources[] sources;
    int sourcesIndex;

    void Start() {
        Events.OnNewQuestion += OnNewQuestion;
        Events.OnPreLoadVideo += SetPreload;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += Prepared;
        videoPlayer.errorReceived += ErrorReceived;

    }

    void ErrorReceived(VideoPlayer vp, string message) {
        Debug.Log("ERROR");
        Debug.Log(message);
        playing = false;
        Events.OnVideoError();
    }

    void SetPreload(PlaylistData.Sources[] s) {
        loading.SetActive(true);
        sources = s;
        sourcesIndex = sources.Length - 1;
    }

    void OnPreLoadVideo() {
        while (sources[sourcesIndex].width == 0 && sourcesIndex > 0)
            sourcesIndex--;

        string file = sources[sourcesIndex].file;

        print("Preloading... " + file);
        videoPlayer.url = file.Replace("https", "http");
        videoPlayer.Prepare();
        loading.SetActive(true);
        if (sourcesIndex > 1)
            StartCoroutine(CheckOnPrepareRate());
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        if (sourcesIndex == 1 && !videoPlayer.isPlaying) {
            VideoPlay();
        }
    }

    IEnumerator CheckOnPrepareRate() {
        Debug.Log("BEGIN: " + prepareTime + " <= " + maxPrepareTime);
        while (prepareTime < maxPrepareTime && !videoPlayer.isPrepared) {
            Debug.Log(prepareTime + " <= " + maxPrepareTime);
            prepareTime += prepareTimeStep;
            yield return new WaitForSecondsRealtime(prepareTimeStep);
        }

        prepareTime = 0f;
        if (!videoPlayer.isPrepared) {
            Debug.Log("not prepared");
            sourcesIndex--;
            if (sourcesIndex < 1) {
                sourcesIndex = 1;
            } else {
                OnPreLoadVideo();
            }
        } else if (!videoPlayer.isPlaying) {
            VideoPlay();
        }
        yield return null;
    }

    void VideoPlay() {
        Debug.Log("Prepared Complete");
        //videoPlayer.timeReference = VideoTimeReference.ExternalTime;
        duration = videoPlayer.length;
        videoPlayer.time = lastTime;
        videoPlayer.Play();
        //loading.SetActive(false);
        Invoke("HideLoading", 0.1f);
        playing = true;
        StartCoroutine(CheckVideoRate());
    }

    IEnumerator CheckVideoRate() {
        Debug.Log("CHECK VIDEO RATE");

        while (playing) {
            //debug.text += lastTime+" : "+ videoPlayer.time+" / ";
            if (lastTime < videoPlayer.time) {
                lastTime = videoPlayer.time;
            } else if (videoPlayer.time > 0) {
                Debug.Log("TIMEOUT: lastT=" + lastTime + " VP=" + videoPlayer.time);
                playing = false;
                videoPlayer.Pause();
                if (sourcesIndex == 1) {
                    NoInternet();
                } else {
                    sourcesIndex--;
                    if (sourcesIndex < 1)
                        sourcesIndex = 1;
                    OnPreLoadVideo();
                }
            }
            yield return new WaitForSecondsRealtime(videoCheckRate);
        }
        yield return null;
    }

    void OnNewQuestion(PlaylistData.VideoData data) {
        print("OnNewQuestion");
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp) {

        // UI.Instance.screensManager.LoadScreen(2, true);
        lastTime = 0;
        playing = false;

        // UI.Instance.screensManager.LoadScreen(2, true);
        if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.questions) {
            Events.OnShowTrivia();
            loading.SetActive(true);
        } else if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.streaming) {
            PlaylistData.Sources[] s = Data.Instance.triviaData.GetNextVideoSource();
            if (s != null) {
                Events.OnPreLoadVideo(s);
            }
            Data.Instance.StartNextQuestion();
        }
    }

    void HideLoading() {
        loading.SetActive(false);
    }

    void NoInternet() {
        Debug.Log("NO HAY INTERNET");
        Events.OnNoConnection();
    }
}
