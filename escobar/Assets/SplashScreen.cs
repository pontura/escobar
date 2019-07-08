using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MainScreen
{
    public Text field;

    public override void OnInit()
    {
        print("Cargando datos del usuario...");
        field.text = "Levantando datos del usuario";
        LoopTillUserLogin();
    }
    void LoopTillUserLogin()
    {        
        if (Data.Instance.userData.userDataInDatabase.uid.Length > 0)
        {            
            print("Cargando capitulos...");
            field.text = "Cargando capítulo...";
            LoopTillCapitulosLoaded();
        }            
        else
            Invoke("LoopTillUserLogin", 0.2f);
    }
    void LoopTillCapitulosLoaded()
    {
       // print("Data.Instance.capitulosData.capitulos.Count" + Data.Instance.capitulosData.capitulos.Count);
        if (Data.Instance.capitulosData.capitulos.Count > 0)
        {
            print("Cargando horario local...");
            field.text = "Cargando horario local...";
            Data.Instance.dateData.GetRealTime();
            // Data.Instance.triviaData.SetTrivia(Data.Instance.capitulosData.capitulos[0].playlistID);
            CapitulosData.Capitulo todayCap = Data.Instance.capitulosData.GetActual();
            if (todayCap == null)
            {
                Debug.LogError("No hay caps hoy");
                Data.Instance.triviaData.SetTriviaNoTriviaToday();
            }
            else
            {
                string playlistIDToday = todayCap.playlistID;
                Data.Instance.triviaData.SetTrivia(playlistIDToday);
            }
            
            CapitulosLoaded();            
        }
        else
            Invoke("LoopTillCapitulosLoaded", 0.2f);
    }
    void CapitulosLoaded()
    {
        if (Data.Instance.dateData.dateTime != null)
        {
            field.text = "";
            print("Horario local done...");
            LoopTillPlaylistLoaded();
        }
        else
            Invoke("CapitulosLoaded", 0.2f);
    }
    void LoopTillPlaylistLoaded() {        
        if (Data.Instance.triviaData.loaded)
            Invoke("ReadyToStart", 2f); 
        else
            Invoke("LoopTillPlaylistLoaded", 0.2f);
    }
    void ReadyToStart()
    {
         UI.Instance.screensManager.LoadScreen(1, true);
    }
}
