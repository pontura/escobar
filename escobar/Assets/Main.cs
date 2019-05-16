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
    public void StartTrining()
    {
        Trivia trivia = UI.Instance.screensManager.all[2] as Trivia;
        trivia.type = Trivia.types.TRAINING;
        UI.Instance.screensManager.LoadScreen(2, true);
    }
}
