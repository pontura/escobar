using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryTraining : MainScreen
{
    public Text title;
    public Text bienField;
    public Text malField;

    public override void OnEnabled()
    {
        int bien = Data.Instance.trainingResults.bien;
        int mal = Data.Instance.trainingResults.mal;        

        if (bien <= mal)
            title.text = "¡Más suerte la próxima!";
        else
            title.text = "¡Muy Bien!";

        bienField.text = bien.ToString();
        malField.text = mal.ToString();

    }
    public void Exit()
    {
        UI.Instance.screensManager.LoadScreen(1, false);
    }
}
