using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminMain : MainScreen
{
    public override void OnInit()
    {
    }
    public void GotoTraining()
    {
        UI.Instance.screensManager.LoadScreen(1, true);
    }
    public void GotoCapitulos()
    {
        UI.Instance.screensManager.LoadScreen(3, true);
    }
}
