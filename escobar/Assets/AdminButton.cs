using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AdminButton : MonoBehaviour
{
    public Text field;
    public System.Action<AdminButton> OnClick;
    public int id;
    public TrainingData.Question questionData;

    public void Init(TrainingData.Question questionData, System.Action<AdminButton> OnClick)
    {
        this.questionData = questionData;
        field.text = questionData.pregunta;
        this.OnClick = OnClick;
    }
    public void OnClicked()
    {
        this.OnClick(this);
    }
}
