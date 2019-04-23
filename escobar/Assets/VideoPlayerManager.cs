using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        Events.OnNewQuestion += OnNewQuestion;
        Events.OnPreLoadVideo += OnPreLoadVideo;
        videoPlayer.loopPointReached += EndReached;
    }
    void OnPreLoadVideo(string file)
    {
        print("Preloading... " + file);
        videoPlayer.url = file;
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {
        print("OnNewQuestion");
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
       // UI.Instance.screensManager.LoadScreen(2, true);
        Events.OnShowTrivia();
    }

}
