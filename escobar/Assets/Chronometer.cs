using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour
{
    bool isOn;
    float totalTime;
    float initialTime;
    public Image bar;
    public Image center;
    public Text field;

    Color centerDefault;

    void Start()
    {
        centerDefault = center.color;
    }

    public void Init(float totalTime)
    {
        initialTime = Time.time;
        this.totalTime = totalTime;
        center.color = Color.black;
        isOn = true;
    }
    void Update()
    {
        if (!isOn)
            return;

        float resto = Time.time - initialTime;
       
        float value = resto / totalTime;
        bar.fillAmount = 1 - value;

        string stringValue = "";
        float seg = totalTime - resto;
        if (seg < 10)
            stringValue = "0" + (int)seg;
        else
            stringValue = ((int)seg).ToString();

        field.text = stringValue;

        if (resto >= totalTime)
        {
            isOn = false;
            field.text = "0";
            bar.fillAmount = 0;
            center.color = Color.red;       
            Events.OnUIFX("timeout");
            Events.OnTriviaTimeOut();            
        }
    }

    public void Pause() {
        isOn = false;
    }
}
