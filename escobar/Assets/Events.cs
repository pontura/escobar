using UnityEngine;

public static class Events {

    
    public static System.Action<string> OnUIFX = delegate { };
    public static System.Action<ButtonStandard> OnButtonClicked = delegate { };
    public static System.Action<string> OnPreLoadVideo = delegate { };
    public static System.Action<PlaylistData.VideoData> OnNewQuestion = delegate { };
    public static System.Action<TrainingData.Question> OnNewTrainingQuestion = delegate { };
    public static System.Action<PlaylistData.VideoData> OnAnswer = delegate { };
    public static System.Action OnShowTrivia = delegate { };
    public static System.Action OnRefreshTrainingData = delegate { };
    public static System.Action OnRefreshCapitulos = delegate { };

    public static System.Action OnTriviaTimeOut = delegate { };
    public static System.Action OnHideTrivia = delegate { };
    public static System.Action<string, Transform> OnTooltip = delegate { };
    public static System.Action<string, System.Action<Firebase.Database.DataSnapshot>> OnGetServerData = delegate { };

    public static System.Action<bool> OnTrainingResponse = delegate { };
    public static System.Action OnTrainingReset = delegate { };
}
