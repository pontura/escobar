using UnityEngine;

public static class Events {

    public static System.Action<string> OnUIFX = delegate { };
    public static System.Action<ButtonStandard> OnButtonClicked = delegate { };
    public static System.Action<PlaylistData.Sources[]> OnPreLoadVideo = delegate { };
    public static System.Action<PlaylistData.VideoData> OnNewQuestion = delegate { };
    public static System.Action<TrainingData.Question> OnNewTrainingQuestion = delegate { };
    public static System.Action<PlaylistData.VideoData> OnAnswer = delegate { };

    public static System.Action OnShowTrivia = delegate { };
    public static System.Action OnRefreshTrainingData = delegate { };

    public static System.Action OnTriviaTimeOut = delegate { };
    public static System.Action OnHideTrivia = delegate { };
    public static System.Action<string, Transform> OnTooltip = delegate { };
    public static System.Action OnFirebaseLogin = delegate { };
    
    public static System.Action<string, System.Action<Firebase.Database.DataSnapshot>> OnGetServerData = delegate { };

    public static System.Action<UsersData.DataBasic> OnUserBasicData = delegate { };
    public static System.Action<bool> OnTrainingResponse = delegate { };
    public static System.Action OnTrainingReset = delegate { };

    public static System.Action OnVideoError = delegate { };
    public static System.Action OnNoConnection = delegate { };

}
