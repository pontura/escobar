﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminMain : MainScreen
{
    public override void OnInit()
    {
    }
    public void GotoTraining()
    {
        UI.Instance.screensManager.LoadScreen(2, true);
    }
}
