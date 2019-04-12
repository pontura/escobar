using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public AudioSpectrum audioSpectrum;
    float audioValue;

    public GameObject mouth;
    public GameObject ClosedMouth;

    public float miniumValue;
    public float scaleFactor;
    public float smooth;

    void Start()
    {
    }

    void Update()
    {
        audioValue = audioSpectrum.result;
        float dest = (1 - audioValue) * scaleFactor;
       // print(dest);

        if (audioValue < miniumValue)
        {
            CloseMouth();
            return;
        } else
        {
            ClosedMouth.SetActive(false);
            mouth.SetActive(true);
            Vector3 s = mouth.transform.localScale;
           
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
