using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public AnimationClip idle;
    public AnimationClip talk;
    public AnimationClip answer;

    public Animator anim;
    public AudioSpectrum audioSpectrum;
    float audioValue;

    public GameObject mouth;
    public GameObject ClosedMouth;

    public float miniumScale;
    public float miniumValue;
    public float scaleFactor;
    public float smooth;

    public void Init()
    {
        audioSpectrum.SetOn();
    }
    public void SetOff()
    {
        audioSpectrum.SetOff();
    }
    public void Idle()
    {
        anim.Play(idle.name);
    }
    public void Talk()
    {
        anim.Play(talk.name);
    }
    public void Answer()
    {
        anim.Play(answer.name);
    }
    void Update()
    {
        audioValue = audioSpectrum.result;
        float dest = (float)((int)(audioValue) * scaleFactor);

        if (dest < miniumValue)
        {
            CloseMouth();
            return;
        } else
        {
            ClosedMouth.SetActive(false);
            mouth.SetActive(true);
            Vector3 s = mouth.transform.localScale;

            if (dest < miniumScale)
                dest = miniumScale;

            s.y = Mathf.Lerp(s.y, dest, smooth);
            mouth.transform.localScale = s; 
        }

    }
    void CloseMouth()
    {
        ClosedMouth.SetActive(true);
        mouth.SetActive(false);
    }
}
