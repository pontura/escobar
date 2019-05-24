using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoTest : MonoBehaviour {


    public int sizeIndex, videoIndex;
    public VideoPlayer videoPlayer;
    //public string baseURL = "https://cdn.jwplayer.com/videos/";
    public string baseURL = "https://content.jwplatform.com/videos/";
    public List<string> sizes;
    public List<string> videos;
    public float videoCheckRate;
    public float maxPrepareTime;
    public float prepareTimeStep;

    public float prepareTime;

    public double duration;

    public double lastTime;

    public GameObject loading;

    public Text debug;

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
        debug.text += "ERROR: " + message;
        playing = false;
    }

    void OnPreLoadVideo(string file) {
        print("Preloading... " + file);
        debug.text="Preloading... " + file;
        videoPlayer.url = file.Replace("https", "http");
        videoPlayer.Prepare();
        loading.SetActive(true);
        if(sizeIndex>0)
            StartCoroutine(CheckOnPrepareRate());
        Debug.Log(videoPlayer.isPrepared);
    }
    
    void EndReached(UnityEngine.Video.VideoPlayer vp) {
        // UI.Instance.screensManager.LoadScreen(2, true);
        Debug.Log("Time: "+videoPlayer.time);
        Debug.Log("End reached");
        lastTime = 0;
        playing = false;
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        if (sizeIndex == 0 && !videoPlayer.isPlaying) {
            VideoPlay();
        }
    }

    IEnumerator CheckOnPrepareRate() {
        Debug.Log("BEGIN: "+prepareTime + " <= " + maxPrepareTime);
        while (prepareTime < maxPrepareTime && !videoPlayer.isPrepared) {
            Debug.Log(prepareTime + " <= " + maxPrepareTime);
            prepareTime += prepareTimeStep;
            yield return new WaitForSecondsRealtime(prepareTimeStep);
        }

        prepareTime = 0f;
        if (!videoPlayer.isPrepared) {
            Debug.Log("not prepared");
            debug.text = "NOT PREPARED";
            sizeIndex--;
            if (sizeIndex < 0) {
                sizeIndex = 0;                
            } else { 
                OnPreLoadVideo(baseURL + videos[videoIndex] + "-" + sizes[sizeIndex] + ".mp4");
            }
            debug.text += "\nQUALITY="+sizeIndex;
        } else if(!videoPlayer.isPlaying) {
            VideoPlay();
        }        
        yield return null;
    }

    IEnumerator CheckVideoRate() {
        Debug.Log("aca");
        debug.text += "CHECK VIDEO RATE";

        while (playing) {
            //debug.text += lastTime+" : "+ videoPlayer.time+" / ";
            if (lastTime < videoPlayer.time) {
                lastTime = videoPlayer.time;
            } else if(videoPlayer.time>0){                
                Debug.Log("TIMEOUT");
                debug.text += "\nTIMEOUT";
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
        debug.text = "PREPARED COMPLETE, QUALITY="+sizeIndex+"\n\n";
        //videoPlayer.timeReference = VideoTimeReference.ExternalTime;
        duration = videoPlayer.length;
        videoPlayer.time = lastTime;
        videoPlayer.Play();
        loading.SetActive(false);
        playing = true;
        StartCoroutine(CheckVideoRate());
    }

    void NoInternet() {
        Debug.Log("NO HAY INTERNET");
        debug.text = "NO HAY INTERNET";
    }

    public void PlayNext() {
        sizeIndex = 3;
        videoIndex++;
        if (videoIndex > videos.Count - 1)
            videoIndex = 0;
        OnPreLoadVideo(baseURL + videos[videoIndex] + "-" + sizes[sizeIndex] + ".mp4");
    }
}
