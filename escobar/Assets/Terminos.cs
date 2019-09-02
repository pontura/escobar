using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminos : MainScreen
{
    public Text field;

    public void GotoMainMenu()
    {
        UI.Instance.screensManager.LoadScreen(1, true);
    }
}
