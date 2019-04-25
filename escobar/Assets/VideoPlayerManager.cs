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
        print("Preloading... " + file);
        videoPlayer.url = file.Replace("https","http");
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {
        loading.SetActive(true);
        print("OnNewQuestion");
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
       // UI.Instance.screensManager.LoadScreen(2, true);
        Events.OnShowTrivia();
    }

    void Prepared(UnityEngine.Video.VideoPlayer vp) {
        loading.SetActive(false);
    }
}
