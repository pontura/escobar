using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MainScreen
{

    public override void OnInit()
    {
        LoopTillloaded();
    }
    void LoopTillloaded()
    {
        if (Data.Instance.triviaData.loaded)
            Done();
        else
            Invoke("LoopTillloaded", 0.1f);
    }
    void Done()
    {
        print("Done");
        if (!Data.Instance.userData.IsLogged())
            UI.Instance.screensManager.LoadScreen(2, true);
       else
            UI.Instance.screensManager.LoadScreen(1, true);
    }
}
