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
        Trivia trivia = UI.Instance.screensManager.all[2] as Trivia;
        trivia.type = Trivia.types.TORNEO;
        string file = Data.Instance.triviaData.GetVideoSource().file;
        Events.OnPreLoadVideo(file);
        UI.Instance.screensManager.LoadScreen(2, true);
    }
    public void StartTrining()
    {
        Events.OnTrainingReset();
        Data.Instance.trainingData.Init();
        Trivia trivia = UI.Instance.screensManager.all[2] as Trivia;
        trivia.type = Trivia.types.TRAINING;
        UI.Instance.screensManager.LoadScreen(2, true);
    }
}
