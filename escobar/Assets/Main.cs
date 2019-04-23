using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MainScreen
{
    public override void OnInit()
    {
    }
    public void StartTrivia()
    {
        string file = Data.Instance.triviaData.GetVideoSource().file;
        Events.OnPreLoadVideo(file);
        UI.Instance.screensManager.LoadScreen(2, true);
    }
}
