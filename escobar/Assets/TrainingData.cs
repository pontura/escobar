using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class TrainingData : MonoBehaviour
{
    public List<Training> entrenamiento;

    [Serializable]
    public class Training
    {
        public string key;
        public Question preguntas;
    }
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

    void Start()
    {
        //PONTURA: cambiar a futuro...
        Invoke("LoadData", 1);

        Events.OnNewTrainingQuestion += OnNewTrainingQuestion;
        Events.OnRefreshTrainingData += OnRefreshTrainingData;
    }
    void OnDestroy()
    {
        Events.OnNewTrainingQuestion -= OnNewTrainingQuestion;
        Events.OnRefreshTrainingData -= OnRefreshTrainingData;
    }
    void LoadData()
    {
        Events.OnGetTrainingQuestions("entrenamiento", OnReady);
    }
    void OnRefreshTrainingData()
    {
        entrenamiento.Clear();
        LoadData();
        activeQuestion = null;
    }
    void OnNewTrainingQuestion(Question q)
    {
        Events.OnShowTrivia();
        questionID++;
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
    }
}
