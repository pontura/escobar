using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class TrainingData : MonoBehaviour
{
    public List<Training> entrenamiento;
    int totalQuestions = 5;
    [Serializable]
    public class Training
    {
        public string key;
        public Question preguntas;
    }
    public int totalDone;
    public int questionID = 0;
    public Training activeQuestion;

    [Serializable]
    public class Question
    {
        public string pregunta;
        public string respuesta_bien;
        public string respuesta_mal_1;
        public string respuesta_mal_2;
    }
    void Awake()
    {
        Events.OnNewTrainingQuestion += OnNewTrainingQuestion;
        Events.OnRefreshTrainingData += OnRefreshTrainingData;
    }
    void Start()
    {
        LoopTillFirebaseReady();
    }
    void LoopTillFirebaseReady()
    {
        if (!Data.Instance.serverManager.isDone)
            Invoke("LoopTillFirebaseReady", 0.25f);
        else
            LoadData();
    }
    
    public void Init()
    {
        totalDone = 0;
    }
    void OnDestroy()
    {
        Events.OnNewTrainingQuestion -= OnNewTrainingQuestion;
        Events.OnRefreshTrainingData -= OnRefreshTrainingData;
    }
    void LoadData()
    {
        Events.OnGetServerData("entrenamiento", OnReady);
        if(!UI.Instance.screensManager.isAdmin)
            LoopToLoadOldTrivias();
    }
    public void OnRefreshTrainingData()
    {
        totalDone = 0;
        entrenamiento.Clear();
        LoadData();
        activeQuestion = null;
    }
    void OnNewTrainingQuestion(Question q)
    {
        
        if(totalDone >= totalQuestions)
        {
            UI.Instance.screensManager.LoadScreen(5, true);
        } else
        {                      
            questionID++;
            if (questionID >= entrenamiento.Count)
                questionID = 0;

            Events.OnShowTrivia();
        }
        totalDone++;
    }
    
    public string[] GetAnswwers()
    {
        string[] arr = new string[3];
        Question question = entrenamiento[questionID].preguntas;
        arr[0] = question.respuesta_bien;
        arr[1] = question.respuesta_mal_1;
        arr[2] = question.respuesta_mal_2;
        return arr;
    }
    public Question GetActualQuestion()
    {
        return entrenamiento[questionID].preguntas;
    }
    void OnReady(DataSnapshot snapshot)
    {
        foreach (DataSnapshot data in snapshot.Children)
        {
            Question tData = new Question();
            IDictionary dataDictiontary = (IDictionary)data.Value;

            tData.pregunta = dataDictiontary["pregunta"].ToString();
            tData.respuesta_bien = dataDictiontary["respuesta_bien"].ToString();
            tData.respuesta_mal_1 = dataDictiontary["respuesta_mal_1"].ToString();
            tData.respuesta_mal_2 = dataDictiontary["respuesta_mal_2"].ToString();

            Training t = new Training();
            t.key = data.Key;
            t.preguntas = tData;

            entrenamiento.Add(t);
        }
        Shuffle(entrenamiento);
    }
    void LoopToLoadOldTrivias()
    {
        if (Data.Instance.capitulosData.capitulos.Count > 0)
        {
            OldTriviasLoaded();
        }
        else
        {
            Invoke("LoopToLoadOldTrivias", 0.5f);
        }
    }
    void OldTriviasLoaded()
    {
        foreach(CapitulosData.Capitulo capitulo in Data.Instance.capitulosData.capitulos)
        {
           if( Data.Instance.dateData.IsFromThePast(capitulo.date))
                Data.Instance.triviaData.LoadPlaylist(capitulo.playlistID, null, false);
        }
    }
    public void AddOldTriviaToTrainingList(PlaylistData.VideoData[] videosData)
    {
        foreach (PlaylistData.VideoData videoData in videosData)
            AddToOldTraining(videoData);

        Shuffle(entrenamiento);
    }
    public void AddToOldTraining(PlaylistData.VideoData videoData)
    {
        Training newTraining = new Training();
        newTraining.preguntas = new Question();
        newTraining.preguntas.pregunta = videoData.title;
        string[] arr = videoData.description.Split("\n"[0]);
        if (arr != null && arr.Length == 3)
        {
            newTraining.preguntas.respuesta_bien = arr[0];
            newTraining.preguntas.respuesta_mal_1 = arr[1];
            newTraining.preguntas.respuesta_mal_2 = arr[2];
            entrenamiento.Add(newTraining);
        }
    }
    void OnOldTriviaDataLoaded(PlaylistData playListData)
    {
        print("____________old " + playListData.playlistID);
    }
    void Shuffle(List<Training> nums)
    {
        if (nums.Count < 2) return;
        for (int a = 0; a < 100; a++)
        {
            int id = UnityEngine.Random.Range(1, nums.Count);
            Training value1 = nums[0];
            Training value2 = nums[id];
            nums[0] = value2;
            nums[id] = value1;
        }
    }
}
