using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia : MainScreen
{
    public states state;

    public enum states
    {           
        QUESTION,
        TRIVIA,
        SUMMARY
    }
    public GameObject triviaContent;
    public Text field;
    public TriviaButton button;
    public Transform container;
    int buttonId = 0;
    public Chronometer chronometer;
    public Character character;
    int[] all;
    public  Animation anim;
    public AnimationClip clip_on;
    public AnimationClip clip_off;

    void Start()
    {
        Events.OnAudioReady += OnAudioReady;
    }
    public override void OnInit()
    {
        anim.Play(clip_on.name);
        Question();
        character.Init();
    }
    void Question()
    {
        character.Talk();
        state = states.QUESTION;            
    }
    void OnAudioReady()
    {
        if (state == states.QUESTION)
        {
            SetTrivia();
        }
        else if (state == states.SUMMARY)
        {
            Data.Instance.StartNextQuestion();
            Question();
        }
    }   
    void SetTrivia()
    {
        character.Idle();
        state = states.TRIVIA;
        anim.Play(clip_off.name);
        triviaContent.SetActive(true);
        all = new int[] { 0, 1, 2 };
        Utils.ShuffleListNums(all);
        Utils.RemoveAllChildsIn(container);
        LoadData(Data.Instance.triviaData.GetActualQuestion());
        chronometer.Init(10);
    }
    void LoadData(JWPlayerData.PlaylistData dataQuestion)
    {
        field.text = dataQuestion.title;
        string[] resp = Data.Instance.triviaData.GetAnswwers();
        AddButton(resp[all[0]], all[0]);
        AddButton(resp[all[1]], all[1]);
        AddButton(resp[all[2]], all[2]);
    }
    void AddButton(string text, int id)
    {
        TriviaButton b = Instantiate(button);
        b.transform.SetParent(container);
        b.transform.localScale = Vector3.one;
        b.Init(this, id, text);
    }
    public void ButtonClicked(int id)
    {
        if (state == states.SUMMARY)
            return;
        state = states.SUMMARY;
        Invoke("Respuesta", 2);
        Data.Instance.userData.SetAnswer(id);
    }
    public void Respuesta()
    {
        character.Answer();
        
        anim.Play(clip_on.name);
        Events.OnAnswer(Data.Instance.triviaData.GetActualQuestion());
    }
}
