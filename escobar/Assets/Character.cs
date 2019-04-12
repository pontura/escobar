using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public AudioSpectrum audioSpectrum;
    float audioValue;

    public GameObject mouth;
    public GameObject ClosedMouth;

    Vector3 originalPosition;
    public float openingFactor;
    public float scaleFactor;
    Vector3 originalsize;

    void Start()
    {
        originalPosition = mouth.transform.localPosition;
        originalsize = mouth.transform.localScale;
    }

    void Update()
    {
        audioValue = audioSpectrum.result;
        if (audioValue < 0.01f)
        {
            CloseMouth();
            return;
        } else
        {
            ClosedMouth.SetActive(false);
            mouth.SetActive(true);
            Vector3 pos = mouth.transform.localPosition;
            pos.y = originalPosition.y += audioValue * openingFactor;
            mouth.transform.localPosition = pos; 
        }

    }
    void CloseMouth()
    {
        ClosedMouth.SetActive(true);
        mouth.SetActive(false);
    }
}
