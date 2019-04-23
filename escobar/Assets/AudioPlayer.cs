using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public float timer;
    public float timerTotal;
    bool isOn;

    void Start()
    {
        Events.OnNewQuestion += OnNewQuestion;
        Events.OnAnswer += OnAnswer;
    }
    void OnDestroy()
    {
        Events.OnNewQuestion -= OnNewQuestion;
        Events.OnAnswer -= OnAnswer;
    }
    void OnAnswer(JWPlayerData.PlaylistData data)
    {
        isOn = true;
        AudioClip clip = data.respuesta;
        audioSource.clip = clip;
        audioSource.Play();
        timerTotal = clip.length;
        timer = 0;
        //string url = Data.Instance.triviaData.GetVideoSource().file;
        //  StartCoroutine( LoadAudio(url) );
    }
    void OnNewQuestion(JWPlayerData.PlaylistData data)
    {
        isOn = true;
        AudioClip clip = data.clip;
        audioSource.clip = clip;
        audioSource.Play();
        timerTotal = clip.length;
        timer = 0;
        //string url = Data.Instance.triviaData.GetVideoSource().file;
        //  StartCoroutine( LoadAudio(url) );
    }
    void Update()
    {
        if (!isOn)
            return;

        timer += Time.deltaTime;
        if(timer>=timerTotal)
        {
            AudioDone();
        }
    }
    void AudioDone()
    {
        isOn = false;
      //  Events.OnAudioReady();
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
