using UnityEngine;

public static class Events {

    
    public static System.Action<string> OnUIFX = delegate { };
    public static System.Action<ButtonStandard> OnButtonClicked = delegate { };
    public static System.Action<JWPlayerData.PlaylistData> OnNewQuestion = delegate { };
    public static System.Action<JWPlayerData.PlaylistData> OnAnswer = delegate { };
    public static System.Action OnAudioReady = delegate { };

    public static System.Action OnTriviaTimeOut = delegate { };

}
