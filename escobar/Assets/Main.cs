using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MainScreen
{
    public GameObject triviaOn;
    public GameObject triviaOff;

    public states state;
    public Text title;
    public Text field;

    public enum states
    {
        TRIVIA_ON,
        TRIVIA_OFF
    }
    public override void OnEnabled()
    {
        state = states.TRIVIA_OFF;
        if(state == states.TRIVIA_ON)
        {
            triviaOn.SetActive(true);
            triviaOff.SetActive(false);
        } else
        {
            title.text = "HOY NO HAY DESAFÍO";
            string date = Data.Instance.capitulosData.GetNext().date;
            field.text = "PRÓX: " + date;
            triviaOn.SetActive(false);
            triviaOff.SetActive(true);
        }
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
