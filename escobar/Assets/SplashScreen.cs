using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MainScreen
{
    public Text field;

    public override void OnInit()
    {
        Events.OnFirebaseLogin();
        print("Cargando datos del usuario...");
        field.text = "Levantando datos del usuario";
        LoopTillUserLogin();
    }
    void LoopTillUserLogin()
    {        
        if (Data.Instance.userData.uid.Length > 0)
        {
            LoopTillCapitulosLoaded();
            print("Cargando capitulos...");
            field.text = "Cargando capítulo...";
        }            
        else
            Invoke("LoopTillUserLogin", 0.2f);
    }
    void LoopTillCapitulosLoaded()
    {        
        if (Data.Instance.capitulosData.capitulos.Count > 0)
        {
            print("Cargando horario local...");
            field.text = "Cargando horario local...";
            Data.Instance.triviaData.SetTrivia(Data.Instance.capitulosData.capitulos[0].playlistID);
            Data.Instance.dateData.GetRealTime();
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
