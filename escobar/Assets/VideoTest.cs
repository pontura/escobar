using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour {


    public int sizeIndex, videoIndex;
    public VideoPlayer videoPlayer;
    public string baseURL = "https://cdn.jwplayer.com/videos/";
    public List<string> sizes;
    public List<string> videos;
    public float videoCheckRate;
    public float maxPrepareTime;
    public float prepareTimeStep;

    public float prepareTime;

    public double duration;

    public double lastTime;

    bool playing;

    void Start() {        
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += Prepared;
        videoPlayer.errorReceived += ErrorReceived;
        videoPlayer.clockResyncOccurred += ClockResync;
        OnPreLoadVideo(baseURL+videos[videoIndex] +"-"+sizes[sizeIndex] +".mp4");
    }

    void ClockResync(VideoPlayer vp, double seconds) {
        Debug.Log(seconds);
    }

    void ErrorReceived(VideoPlayer vp, string message) {
        Debug.Log("ERROR");
        Debug.Log(message);
        playing = false;
    }

    void OnPreLoadVideo(string file) {
        print("Preloading... " + file);
        videoPlayer.url = file.Replace("https", "http");
        videoPlayer.Prepare();
        StartCoroutine(CheckOnPrepareRate());
        Debug.Log(videoPlayer.isPrepared);
    }
    
    void EndReached(UnityEngine.Video.VideoPlayer vp) {
        // UI.Instance.screensManager.LoadScreen(2, true);
        Debug.Log("Time: "+videoPlayer.time);
        Debug.Log("End reached");
        playing = false;
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        if (sizeIndex == 0 && !videoPlayer.isPlaying) {
            VideoPlay();
        }
    }

    IEnumerator CheckOnPrepareRate() {
        while (prepareTime < maxPrepareTime && !videoPlayer.isPrepared) {
            Debug.Log(prepareTime + " <= " + maxPrepareTime);
            prepareTime += prepareTimeStep;
            yield return new WaitForSecondsRealtime(prepareTime);
        }

        if (!videoPlayer.isPrepared) {
            Debug.Log("not prepared");
            sizeIndex--;
            if (sizeIndex < 0) {
                sizeIndex = 0;
                Debug.Log("Next");
            } else { 
                OnPreLoadVideo(baseURL + videos[videoIndex] + "-" + sizes[sizeIndex] + ".mp4");
            }
            
        } else if(!videoPlayer.isPlaying) {
            VideoPlay();
        }

        prepareTime = 0;
        yield return null;
    }

    IEnumerator CheckVideoRate() {
        Debug.Log("aca");
        while (playing) {            
            if (lastTime < videoPlayer.time) {
                lastTime = videoPlayer.time;
            } else if(videoPlayer.time>0){
                Debug.Log("TIMEOUT");
                Debug.Log(videoPlayer.isPlaying);
                playing = false;
                if (sizeIndex == 0) {                    
                    NoInternet();
                } else {
                    sizeIndex--;
                    if (sizeIndex < 0)
                        sizeIndex = 0;
                    OnPreLoadVideo(baseURL + videos[videoIndex] + "-" + sizes[sizeIndex] + ".mp4");
                }
            }            
            yield return new WaitForSecondsRealtime(videoCheckRate);
        }        
        yield return null;
    }

    void VideoPlay() {
        Debug.Log("Prepared Complete");
        //videoPlayer.timeReference = VideoTimeReference.ExternalTime;
        duration = videoPlayer.length;
        videoPlayer.Play();
        playing = true;
        StartCoroutine(CheckVideoRate());
    }

    void NoInternet() {
        Debug.Log("NO HAY INTERNET");
    }

}
