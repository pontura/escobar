using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestionLine : MonoBehaviour
{
    public Text field;

    public Text bien;
    public Text mal_1;
    public Text mal_2;

    public System.Action<PlaylistData.VideoData> OnClick;
    PlaylistData.VideoData data;
    public int id;

    public void Init(PlaylistData.VideoData d, System.Action<PlaylistData.VideoData> OnClick)
    {
        this.OnClick = OnClick;
        this.data = d;
        field.text = d.title;
        string[] arr = d.description.Split("\n"[0]);
        if (arr == null || arr.Length < 3)
        {
            bien.text = "";
            mal_1.text = "";
            mal_2.text = "";
        }
        else
        {
            bien.text = arr[0];
            mal_1.text = arr[1];
            mal_2.text = arr[2];
        }
    }
    public void OnClicked()
    {
        this.OnClick(data);
    }
}
