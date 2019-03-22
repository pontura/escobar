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
    void OnNewQuestion(Data.Question dataQuestion)
    {
        videoPlayer.url = dataQuestion.video_url;
        videoPlayer.Play();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        UI.Instance.screensManager.LoadScreen(1);
    }

}
