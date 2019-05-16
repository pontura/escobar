using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class JWPlayerData : MonoBehaviour
{
    public List<string> playlist_URL;

    public string streamingURL = "ehFXPETl";

    public int questionID;

    public JWData data;

    public SOURCE source;
    public enum SOURCE {
        unloaded,
        questions,
        streaming
    }

    [Serializable]
    public class JWData
    {
        public VideoData[] playlist;
        public string date;
        public string time;
    }
    [Serializable]
    public class VideoData
    {
        public string title;
        public string description;
        public string image;
        public Sources[] sources;
    }
    [Serializable]
    public class Sources
    {
        public int width;
        public string file;
    }
    public bool loaded;

    public bool DontLoadData;

    void Start()
    {
        if (!DontLoadData)
            SetTrivia();
    }    

    public void SetTrivia() {
        questionID = 0;
        if (source == SOURCE.questions)
            return;
        StartCoroutine(LoadFromJWPlayer(playlist_URL[0]));
        source = SOURCE.questions;
    }

    public void SetStreaming() {
        questionID = 0;
        if (source == SOURCE.streaming)
            return;
        StartCoroutine(LoadFromJWPlayer(streamingURL));
        source = SOURCE.streaming;
    }

    IEnumerator LoadFromJWPlayer(string url)
    {
        Debug.Log("aca");
        WWW www = new WWW("https://cdn.jwplayer.com/v2/playlists/"+url);
        yield return www;
        data = JsonUtility.FromJson<JWData>(www.text);
        loaded = true;

        if (source == SOURCE.streaming) {
            string file = Data.Instance.triviaData.GetVideoSource().file;
            Events.OnPreLoadVideo(file);
            UI.Instance.screensManager.LoadScreen(2, false);
        }
    }
    public VideoData GetActualQuestion()
    {
        return data.playlist[questionID];
    }
    public string[] GetAnswwers()
    {
        VideoData question = GetActualQuestion();
        return question.description.Split("\n"[0]);
    }
    public Sources GetVideoSource()
    {
        foreach(Sources source in GetActualQuestion().sources)
        {
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID].sources[1];
    }
    public Sources GetNextVideoSource()
    {
        if(data.playlist.Length <= questionID+1)
            return null;

        VideoData nextData = data.playlist[questionID+1];
        foreach (Sources source in nextData.sources)
        {
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID+1].sources[1];
    }
}
