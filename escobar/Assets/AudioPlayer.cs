using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;


    void Start()
    {
        Events.OnNewQuestion += OnNewQuestion;
    }
    void OnDestroy()
    {
        Events.OnNewQuestion -= OnNewQuestion;
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {
        AudioClip clip = data.clip;
        audioSource.clip = clip;
        audioSource.Play();
        //string url = Data.Instance.triviaData.GetVideoSource().file;
      //  StartCoroutine( LoadAudio(url) );
    }

    IEnumerator LoadAudio(string URL)
    {
        UnityWebRequest music = UnityWebRequestMultimedia.GetAudioClip(URL, AudioType.MPEG);
        yield return music.SendWebRequest();

        if (music.isNetworkError)
        {
            Debug.Log(music.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(music);
            Debug.Log(clip + " length: " + clip.length);
            if (clip)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
