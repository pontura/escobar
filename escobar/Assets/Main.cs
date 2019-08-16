using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MainScreen
{
    public GameObject triviaOn;
    public GameObject triviaOff;
    public GameObject liveStreaming;

    public Text title;
    public Text field;

    public override void OnEnabled()
    {
        LoopTillTimeLoaded();
    }
    void LoopTillTimeLoaded()
    {
        if (Data.Instance.dateData.dateTime == null)
        {
            Invoke("LoopTillTimeLoaded", 0.1f);
        }
        else
        {
            TimeLoaded();
        }
    }
    void TimeLoaded()
    {
        CapitulosData.Capitulo cap = Data.Instance.capitulosData.GetActual();

        // print(Data.Instance.capitulosData.activeCapitulo.key + " --------------- " + Data.Instance.userData.lastChapterPlayedKey);
        triviaOn.SetActive(false);
        triviaOff.SetActive(false);
        liveStreaming.SetActive(false);

        if (cap == null)
        {
            triviaOff.SetActive(true);
            title.text = "HOY NO HAY DESAFÍO";
            CapitulosData.Capitulo newCap = Data.Instance.capitulosData.GetNext();
            if (newCap == null)
            {
                field.text = "Aún no hay nuevos capítulos";
                return;
            }
            string date = newCap.date;
            field.text = "Próx: " + date;
        }
        else //if (Data.Instance.capitulosData.activeCapitulo.key == Data.Instance.userData.lastChapterPlayedKey)
        {
            string timeLive = Data.Instance.capitulosData.activeCapitulo.time;

            if (Data.Instance.dateData.dateTime.Hour.ToString() == timeLive)
            {
                liveStreaming.SetActive(true);
            }
            else if (Data.Instance.dateData.dateTime.Hour > int.Parse(Data.Instance.capitulosData.activeCapitulo.time))
            {
                triviaOff.SetActive(true);
                title.text = "Transmisión terminada";
                field.text = "Vuelve mañana para un nuevo Desafío";
            }
            else if (Data.Instance.capitulosData.activeCapitulo.key == Data.Instance.userData.lastChapterPlayedKey)
            {
                triviaOff.SetActive(true);
                title.text = "¡Ya jugaste!";
                field.text = "La transmisión es a las " + Data.Instance.capitulosData.activeCapitulo.time + " hs.";
            }
            else
            {
                triviaOn.SetActive(true);
            }
        }
        
    }
    public void StartTrivia()
    {
        Trivia trivia = UI.Instance.screensManager.all[2] as Trivia;
        trivia.type = Trivia.types.TORNEO;        
        Events.OnPreLoadVideo(Data.Instance.triviaData.GetVideoSource());
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
    public void OpenLiveStreaming()
    {
        Application.OpenURL("https://desafioceli-9e8f4.firebaseapp.com/");
    }
}
