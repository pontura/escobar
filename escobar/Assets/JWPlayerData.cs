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

    public PlaylistData data;

    public SOURCE source;

    public enum SOURCE {
        unloaded,
        questions,
        streaming
    }
    
    public bool loaded;
    public bool DontLoadData;

    void Start()
    {
        if (!DontLoadData)
            SetTrivia();
    }
    System.Action OnLoaded;
    public void LoadPlaylist(string playlistID, System.Action OnLoaded)
    {
        StopAllCoroutines();
        StartCoroutine(LoadFromJWPlayer(playlistID));
        this.OnLoaded = OnLoaded;
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

    IEnumerator LoadFromJWPlayer(string playlistID)
    {
        Debug.Log("LoadFromJWPlayer " + playlistID);
        WWW www = new WWW("https://cdn.jwplayer.com/v2/playlists/"+ playlistID);
        yield return www;
        data = JsonUtility.FromJson<PlaylistData>(www.text);
        loaded = true;

        if (OnLoaded != null)
        {            
            OnLoaded();
            OnLoaded = null;
        }

        //if (source == SOURCE.streaming) {
        //    string file = Data.Instance.triviaData.GetVideoSource().file;
        //    Events.OnPreLoadVideo(file);
        //    UI.Instance.screensManager.LoadScreen(2, false);
        //}
    }
    public PlaylistData.VideoData GetActualQuestion()
    {
        return data.playlist[questionID];
    }
    public string[] GetAnswwers()
    {
        PlaylistData.VideoData question = GetActualQuestion();
        return question.description.Split("\n"[0]);
    }
    public PlaylistData.Sources GetVideoSource()
    {
        foreach (PlaylistData.Sources source in GetActualQuestion().sources)
        {
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID].sources[1];
    }
    public PlaylistData.Sources GetNextVideoSource()
    {
        if(data.playlist.Length <= questionID+1)
            return null;

        PlaylistData.VideoData nextData = data.playlist[questionID+1];
        foreach (PlaylistData.Sources source in nextData.sources)
        {
            if (source.width == 270)
                return source;
        }
        return data.playlist[questionID+1].sources[1];
    }
    public void Open_URL(string playlistID)
    {
        string url = "https://dashboard.jwplayer.com/#/content/playlists/manual_detail?key=" + playlistID;
        Application.OpenURL(url);
    }
    
}
