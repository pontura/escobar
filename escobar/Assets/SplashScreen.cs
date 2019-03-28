using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : ScreenBase
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
            UI.Instance.screensManager.LoadScreen("Register");
       else
            UI.Instance.screensManager.LoadScreen("Main");
    }
}
