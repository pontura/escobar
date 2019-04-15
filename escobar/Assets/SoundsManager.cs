using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource audiosSource;
    public AudioClip si;
    public AudioClip no;
    public AudioClip timeout;

    void Start()
    {
        Events.OnUIFX += OnUIFX;
    }

    void OnUIFX(string name)
    {
        AudioClip clip = null;         
        switch(name)
        {
            case "si":
                clip = si;
                break;
            case "no":
                clip = no;
                break;
            case "timeout":
                clip = timeout;
                break;
        }
        if (clip == null)
            return;

        audiosSource.clip = clip;
        audiosSource.Play();
    }
}
