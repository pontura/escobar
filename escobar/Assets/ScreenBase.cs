using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    [HideInInspector]
    public string screenName;

    public void Init()
    {
        gameObject.SetActive(true);
        OnInit();
    }
    public virtual void OnInit() {  }
}
