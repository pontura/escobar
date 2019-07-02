using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class JWPlayerData : MonoBehaviour
{
  //  public List<string> playlist_URL;

    //public string streamingURL = "ehFXPETl";

    public int questionID;

    public PlaylistData data;

    public List<PlaylistData> oldData;

    public SOURCE source;

    public enum SOURCE {
        unloaded,
        questions,
        streaming
    }
    
    public bool loaded;
    public bool DontLoadData;

    System.Action OnLoaded;
    public void LoadPlaylist(string playlistID, System.Action OnLoaded, bool isTodayTrivia)
    {        
        //StopAllCoroutines();
        StartCoroutine(LoadFromJWPlayer(playlistID, isTodayTrivia));
        this.OnLoaded = OnLoaded;
    }
    public void SetTrivia(string playlistID) {
        questionID = 0;
        if (source == SOURCE.questions)
            return;
        StartCoroutine(LoadFromJWPlayer(playlistID, true));
        source = SOURCE.questions;
    }
    public void SetTriviaNoTriviaToday()
    {
        loaded = true;
    }
    public void Restart()
    {
        questionID = 0; ;
    }
    //public void SetStreaming() {
    //    questionID = 0;
    //    if (source == SOURCE.streaming)
    //        return;
    //    StartCoroutine(LoadFromJWPlayer(streamingURL));
    //    source = SOURCE.streaming;
    //}

    IEnumerator LoadFromJWPlayer(string playlistID, bool isTodayTrivia)
    {
        string url = "https://cdn.jwplayer.com/v2/playlists/" + playlistID;
        WWW www = new WWW(url);
        Debug.Log("LoadFromJWPlayer " + playlistID +  "url: " + url  + " isTodayTrivia : " + isTodayTrivia);

        yield return www;
        if (!isTodayTrivia)
        {
            Debug.Log("old: playlist"  + playlistID + "  oldData.Count: " +  oldData.Count);
            PlaylistData d = JsonUtility.FromJson<PlaylistData>(www.text);
            oldData.Add(d);
            Data.Instance.trainingData.AddOldTriviaToTrainingList(d.playlist);
        }
        else
        {
            data = JsonUtility.FromJson<PlaylistData>(www.text);
            loaded = true;

            if (OnLoaded != null)
            {
                OnLoaded();
                OnLoaded = null;
            }
        }
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
    public PlaylistData.Sources[] GetVideoSource()
    {
        return data.playlist[questionID].sources;
    }
    public PlaylistData.Sources[] GetNextVideoSource()
    {
        if(data.playlist.Length <= questionID+1)
            return null;

        PlaylistData.VideoData nextData = data.playlist[questionID+1];        
        return data.playlist[questionID+1].sources;
    }
    public void Open_URL(string playlistID)
    {
        string url = "https://dashboard.jwplayer.com/#/content/playlists/manual_detail?key=" + playlistID;
        Application.OpenURL(url);
    }
    public void OPEN_VIDEO_EDIT(string videoID)
    {        
        string url = "https://dashboard.jwplayer.com/#/content/detail?key=" + videoID;
        Application.OpenURL(url);
    }
    
}
