using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : ScreenBase
{
    public override void OnInit()
    {
    }
    public void StartTrivia()
    {
        //UI.Instance.screensManager.Reset();
        Events.OnNewQuestion(Data.Instance.triviaData.GetActualQuestion());
    }
}
