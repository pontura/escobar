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
        //UI.Instance.screensManager.Reset();
        Events.OnNewQuestion(Data.Instance.triviaData.GetActualQuestion());
        UI.Instance.screensManager.LoadScreen(2, true);
    }
}
