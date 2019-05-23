using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MainScreen
{

    public override void OnInit()
    {
        LoopTillCapitulosLoaded();
    }
    void LoopTillCapitulosLoaded()
    {
        if (Data.Instance.capitulosData.capitulos.Count > 0)
        {
            //DEMO: carga el primero de la lista
            Data.Instance.triviaData.SetTrivia(Data.Instance.capitulosData.capitulos[0].playlistID);
            LoopTillPlaylistLoaded();
        }
        else
            Invoke("LoopTillCapitulosLoaded", 0.2f);
    }
    void LoopTillPlaylistLoaded() {
        if (Data.Instance.triviaData.loaded)
            Invoke("ReadyToStart", 2f); 
        else
            Invoke("LoopTillPlaylistLoaded", 0.2f);
    }
    void ReadyToStart()
    {
        if (!Data.Instance.userData.IsLogged())
            UI.Instance.screensManager.LoadScreen(3, true); 
       else
            UI.Instance.screensManager.LoadScreen(1, true);
    }
}
