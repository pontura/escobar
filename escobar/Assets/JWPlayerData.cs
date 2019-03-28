using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class JWPlayerData : MonoBehaviour
{

    public int questionID;

    public JWData data;

    [Serializable]
    public class JWData
    {
        public PlaylistData[] playlist;
    }
    [Serializable]
    public class PlaylistData
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

    void Start()
    {
        StartCoroutine(LoadFromJWPlayer());
    }
    IEnumerator LoadFromJWPlayer()
    {
        WWW www = new WWW("https://cdn.jwplayer.com/v2/playlists/vYe6YRHr");
        yield return www;
        data = JsonUtility.FromJson<JWData>(www.text);
        loaded = true;
    }
    public PlaylistData GetActualQuestion()
    {
        return data.playlist[questionID];
    }
    public string[] GetAnswwers()
    {
        PlaylistData question = GetActualQuestion();
        return question.description.Split("\n"[0]);
    }
    public Sources GetVideoSource()
    {
        foreach(Sources source in GetActualQuestion().sources)
        {
            if (source.width == 1080)
                return source;
        }
        return data.playlist[questionID].sources[0];
    }
}
