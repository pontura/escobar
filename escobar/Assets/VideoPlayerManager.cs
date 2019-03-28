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
        videoPlayer.loopPointReached += EndReached;
    }
    void OnDestroy()
    {
        Events.OnNewQuestion -= OnNewQuestion;
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {
        videoPlayer.url = Data.Instance.triviaData.GetVideoSource().file;
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        UI.Instance.screensManager.LoadScreen(2);
    }

}
