using UnityEngine;
using System.Collections;
using System;

public class Data : MonoBehaviour {

    static Data mInstance = null;

    public int questionID;
    public Trivia trivia;

    [Serializable]
    public class Trivia
    {
        public Question[] questions;
    }

    [Serializable]
    public class Question
    {
        public string video_url;
        public string question;
        public string answer1;
        public string answer2;
        public string answer3;
    }
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
        mInstance = this;
		DontDestroyOnLoad(this);
	}
    public void StartNextQuestion()
    {
        questionID++;
        Events.OnNewQuestion( GetActualQuestion() );
    }
    public Question GetActualQuestion()
    {
        return trivia.questions[questionID];
    }
}
