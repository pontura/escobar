using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject loading;

    void Start()
    {
        Events.OnNewQuestion += OnNewQuestion;
        Events.OnPreLoadVideo += OnPreLoadVideo;
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += Prepared;

    }
    void OnPreLoadVideo(string file)
    {
        loading.SetActive(true);
        print("Preloading... " + file);
        videoPlayer.url = file.Replace("https","http");
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {        
        print("OnNewQuestion");
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        // UI.Instance.screensManager.LoadScreen(2, true);
        if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.questions) {            
            Events.OnShowTrivia();
            loading.SetActive(true);
        } else if (Data.Instance.triviaData.source == JWPlayerData.SOURCE.streaming) {
            JWPlayerData.Sources s = Data.Instance.triviaData.GetNextVideoSource();
            if (s != null) {
                string file = s.file;
                Events.OnPreLoadVideo(file);
            }
            Data.Instance.StartNextQuestion();            
        }
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        Invoke("HideLoading", 0.1f);
    }

    void HideLoading() {
        loading.SetActive(false);
    }
}
