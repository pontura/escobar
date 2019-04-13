using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summary : MainScreen
{
    public override void OnInit()
    {
        
    }
    public void Replay()
    {
        Data.Instance.userData.Reset();
        Data.Instance.GetComponent<JWPlayerData>().questionID = 0;
        UI.Instance.screensManager.LoadScreen(1, false);
    }
}
