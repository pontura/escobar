using UnityEngine;
using System.Collections;
using System;

public class UI : MonoBehaviour
{
    static UI mInstance = null;
    [HideInInspector]
    public ScreensManager screensManager;

    public static UI Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a UI antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Start()
    {
        mInstance = this;
        screensManager = GetComponent<ScreensManager>();
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
