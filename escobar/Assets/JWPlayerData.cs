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
        public AudioClip clip;
        public AudioClip respuesta;
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
        if(!DontLoadData)
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
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID].sources[1];
    }
    public Sources GetNextVideoSource()
    {
        if(data.playlist.Length <= questionID+1)
            return null;

        PlaylistData nextData = data.playlist[questionID+1];
        foreach (Sources source in nextData.sources)
        {
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID+1].sources[1];
    }
}
