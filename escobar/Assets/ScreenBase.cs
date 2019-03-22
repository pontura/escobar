using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
        OnInit();
    }
    public virtual void OnInit() {  }
}
