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
        Data.Instance.triviaData.SetTrivia();
        UI.Instance.screensManager.LoadScreen(1, false);
    }

    public void Streaming() {
        Data.Instance.userData.Reset();
        Data.Instance.triviaData.SetStreaming();        
    }
}
