using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingResults : MonoBehaviour
{
    public int bien;
    public int mal;

    void Start()
    {
        Events.OnTrainingResponse += OnTrainingResponse;
        Events.OnTrainingReset += OnTrainingReset;
    }

    void OnDestroy()
    {
        Events.OnTrainingResponse -= OnTrainingResponse;
        Events.OnTrainingReset -= OnTrainingReset;
    }
    void OnTrainingResponse(bool isGood)
    {
        if (isGood)
            bien++;
        else
            mal++;
    }
    void OnTrainingReset()
    {
        bien = 0;
        mal = 0;
    }
}
