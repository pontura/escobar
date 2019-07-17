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

    float prepareTime;
    double duration;
    double lastTime;

    bool playing;

    bool waiting2Play;

    PlaylistData.Sources[] sources;
    int sourcesIndex;
    int timeoutTrivia = 10;

    void Start() {
        Events.OnNewQuestion += OnNewQuestion;
        Events.OnPreLoadVideo += SetPreload;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += Prepared;
        videoPlayer.errorReceived += ErrorReceived;
    }

    void ErrorReceived(VideoPlayer vp, string message) {
        Debug.Log(message);
        playing = false;
        Events.OnVideoError();
    }

    void SetPreload(PlaylistData.Sources[] s) {
        loading.SetActive(true);
        sources = s;     
        
        sourcesIndex = 2;
        // le agrego que arranque por la segunda:       

        if (sourcesIndex < 1)
            sourcesIndex = 1;

        OnPreLoadVideo();
    }

    void OnPreLoadVideo() {
        while (sources[sourcesIndex].width == 0 && sourcesIndex > 0)
            sourcesIndex--;

        string file = sources[sourcesIndex].file;

       // print("Preloading... " + file);
        videoPlayer.url = file.Replace("https", "http");
        videoPlayer.Prepare();
        //loading.SetActive(true);
        StopAllCoroutines();
        if (sourcesIndex > 1)
            StartCoroutine(CheckOnPrepareRate());
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        /*if (sourcesIndex == 1 && !videoPlayer.isPlaying)
        {
            VideoPlay();
        } */       
        if(waiting2Play)
            VideoPlay();
    }

    IEnumerator CheckOnPrepareRate() {
        while (prepareTime < maxPrepareTime && !videoPlayer.isPrepared) {
           // Debug.Log(prepareTime + " <= " + maxPrepareTime);
            prepareTime += prepareTimeStep;
            yield return new WaitForSecondsRealtime(prepareTimeStep);
        }
        prepareTime = 0f;
        if (!videoPlayer.isPrepared) {
           // Debug.Log("not prepared");
            sourcesIndex--;
            if (sourcesIndex < 1) {
                sourcesIndex = 1;
            } else {
                OnPreLoadVideo();
            }
        } else if (!videoPlayer.isPlaying) {
            //VideoPlay();
        }
        yield return null;
    }

    void VideoPlay() {
      //  Debug.Log("Prepared Complete");
        waiting2Play = false;
        //videoPlayer.timeReference = VideoTimeReference.ExternalTime;
        duration = videoPlayer.length;
        videoPlayer.time = lastTime;
        videoPlayer.Play();
        loading.SetActive(false);
        playing = true;
        StopAllCoroutines();
        StartCoroutine(CheckVideoRate());
    }
    bool triviaShowed;
    void CheckForTriviaShow(int _time)
    {
        if (triviaShowed)
            return;      
       // print("CheckTime: " + _time +  " duration " + data.duration);
        if (_time > data.duration - timeoutTrivia)
        {
            triviaShowed = true;
            Events.OnShowTrivia();
        }
    }
    IEnumerator CheckVideoRate() {
      //  Debug.Log("CHECK VIDEO RATE");

        while (playing) {
            double videoTimer = videoPlayer.time;
            CheckForTriviaShow((int)videoTimer);            
            //debug.text += lastTime+" : "+ videoPlayer.time+" / ";
            if (lastTime < videoTimer) {
                lastTime = videoTimer;
            } else if (videoTimer > 0) {
              //  Debug.Log("TIMEOUT: lastT=" + lastTime + " VP=" + videoTimer);
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
    PlaylistData.VideoData data;
    void OnNewQuestion(PlaylistData.VideoData data) {
        lastTime = 0;
        //videoPlayer.Stop();
        loading.SetActive(true);
        playing = false;
        triviaShowed = false;
        this.data = data;
        if (videoPlayer.isPrepared)
            VideoPlay();
        else
            waiting2Play = true;
    }


    void EndReached(UnityEngine.Video.VideoPlayer vp) {
        playing = false;
        // UI.Instance.screensManager.LoadScreen(2, true);
        return;

        // UI.Instance.screensManager.LoadScreen(2, true);
        if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.questions) {
           // Events.OnShowTrivia();
          //  loading.SetActive(true);
        } else if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.streaming) {
            PlaylistData.Sources[] s = Data.Instance.triviaData.GetNextVideoSource();
            if (s != null) {
                Events.OnPreLoadVideo(s);
            }
            Data.Instance.StartNextQuestion();
        }
    }

    void NoInternet() {
        Debug.Log("NO HAY INTERNET");
        Events.OnNoConnection();
    }
}
