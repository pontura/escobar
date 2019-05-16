using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MainScreen
{

    public override void OnInit()
    {
        LoopTillloaded();
    }
    void LoopTillloaded() {
        if (Data.Instance.triviaData.loaded)
            Invoke("Done", 2f); 
        else
            Invoke("LoopTillloaded", 0.1f);
    }
    void Done()
    {
        if (!Data.Instance.userData.IsLogged())
            UI.Instance.screensManager.LoadScreen(3, true); 
       else
            UI.Instance.screensManager.LoadScreen(1, true);
    }
}
