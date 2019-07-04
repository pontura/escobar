using UnityEngine;
using System.Collections;
using System;

public class Data : MonoBehaviour {

    static Data mInstance = null;

    [HideInInspector]
    public ServerManager serverManager;
    public JWPlayerData triviaData;
    public UserData userData;
    public CapitulosData capitulosData;
    public TrainingData trainingData;
    public TrainingResults trainingResults;
    public ResultsData resultsData;
    public DateData dateData;
    public UsersData usersData;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
	void Awake () {
       // PlayerPrefs.DeleteAll();
        mInstance = this;
		DontDestroyOnLoad(this);
        serverManager = GetComponent<ServerManager>();
        userData = GetComponent<UserData>();
        trainingData = GetComponent<TrainingData>();
        capitulosData = GetComponent<CapitulosData>();
        trainingResults = GetComponent<TrainingResults>();
        dateData = GetComponent<DateData>();
        resultsData = GetComponent<ResultsData>();
        usersData = GetComponent<UsersData>();
    }
    public void StartNextQuestion()
    {
        if (triviaData.questionID < Data.Instance.triviaData.data.playlist.Length - 1)
        {
            triviaData.questionID++;
            Events.OnNewQuestion(triviaData.GetActualQuestion());
        } else
        {
            Data.Instance.serverManager.Send();
            UI.Instance.screensManager.LoadScreen(4, true);
            Events.OnHideTrivia();
        }
    }
}
