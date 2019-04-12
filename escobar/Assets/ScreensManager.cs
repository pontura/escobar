using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public ScreenBase[] screens;
    public int id;

    void Start()
    {
        foreach (ScreenBase s in screens)
            s.screenName = s.name;

        Reset();
        LoadScreen(0);
    }
    void Reset()
    {
        foreach (ScreenBase s in screens)
            s.gameObject.SetActive(false);
    }
    public void LoadScreen(int _id)
    {
        Reset();
        id = _id;
        screens[id].Init();
    }
    public void LoadScreen(string _screenName)
    {
        Reset();
        foreach(ScreenBase sb in screens)
        {
            print(sb.screenName + _screenName);
            if (sb.screenName == _screenName)
            {
                
                sb.Init();
                return;
            }
        }       
    }
    public void Next()
    {
        Reset();
        id++;
        screens[id].Init();
    }
}
