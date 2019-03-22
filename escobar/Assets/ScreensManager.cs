using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public ScreenBase[] screens;

    void Start()
    {
        Reset();
        LoadScreen(0);
    }
    public void Reset()
    {
        foreach (ScreenBase s in screens)
            s.gameObject.SetActive(false);
    }
    public void LoadScreen(int id)
    {
        Reset();
        screens[id].Init();
    }
}
