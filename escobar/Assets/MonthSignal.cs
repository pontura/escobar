using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthSignal : MonoBehaviour
{
    public Text field;

    public void Init(string value)
    {
        field.text = value;
    }
}
