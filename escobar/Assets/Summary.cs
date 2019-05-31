using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MainScreen
{
    public Text field;

    public override void OnEnabled()
    {
        field.text = "No te pierdas la transmision en vivo a las " + Data.Instance.capitulosData.activeCapitulo.time + " horas.";
    }
    public void Replay()
    {
        Data.Instance.userData.Reset();
        Data.Instance.triviaData.Restart();
        //Data.Instance.triviaData.SetTrivia(Data.Instance.triviaData.streamingURL);
        UI.Instance.screensManager.LoadScreen(1, false);
    }

    public void Streaming() {
        Data.Instance.userData.Reset();
      //  Data.Instance.triviaData.SetStreaming();        
    }
}
