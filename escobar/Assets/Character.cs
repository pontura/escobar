using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
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
           
            s.y = Mathf.Lerp(s.y, dest+ miniumScale, smooth);
            mouth.transform.localScale = s; 
        }

    }
    void CloseMouth()
    {
        ClosedMouth.SetActive(true);
        mouth.SetActive(false);
    }
}
