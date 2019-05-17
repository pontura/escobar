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

    public void Init(int id, string text, System.Action<AdminButton> OnClick)
    {
        this.id = id;
        field.text = text;
        this.OnClick = OnClick;
    }
    public void OnClicked()
    {
        this.OnClick(this);
    }
}
