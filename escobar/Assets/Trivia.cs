using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia : MainScreen
{
    public GameObject timeOut;
    public Text field;
    public TriviaButton button;
    public Transform container;
    int buttonId = 0;
    public Chronometer chronometer;
    public GameObject content;
    int[] all;
    public List<TriviaButton> buttons;
    public bool done;
    public GameObject loading;

    void Start()
    {
        
        OnHideTrivia();
        Events.OnShowTrivia += OnShowTrivia;
        Events.OnTriviaTimeOut += OnTriviaTimeOut;
        Events.OnHideTrivia += OnHideTrivia;
    }

    private void OnDestroy() {
        Events.OnShowTrivia -= OnShowTrivia;
        Events.OnTriviaTimeOut -= OnTriviaTimeOut;
        Events.OnHideTrivia -= OnHideTrivia;
    }

    public override void OnInit()
    {
        timeOut.SetActive(false);
        Events.OnNewQuestion(Data.Instance.triviaData.GetActualQuestion());        
    }
    void OnTriviaTimeOut()
    {
        done = true;
        Invoke("TimeOutDelayed", 0.4f);
        Data.Instance.userData.SetAnswer(-1);
        StartCoroutine(ResetTrivia(0));
    }
    void TimeOutDelayed()
    {
        timeOut.SetActive(true);
    }
    void OnShowTrivia()
    {
        buttons.Clear();
        done = false;
        content.SetActive(true);
        all = new int[] { 0, 1, 2 };
        Utils.ShuffleListNums(all);
        Utils.RemoveAllChildsIn(container);
        LoadData(Data.Instance.triviaData.GetActualQuestion());
        chronometer.Init(10);
        Invoke("DelayToPreload", 0.5f);
    }
    void DelayToPreload()
    {
        JWPlayerData.Sources s = Data.Instance.triviaData.GetNextVideoSource();
        if (s != null)
        {
            string file = s.file;
            Events.OnPreLoadVideo(file);
        }
    }
    void OnHideTrivia()
    {        
        content.SetActive(false);
    }
    void LoadData(JWPlayerData.PlaylistData dataQuestion)
    {
        field.text = dataQuestion.title;
        string[] resp = Data.Instance.triviaData.GetAnswwers();
        AddButton(resp[all[0]], all[0]);
        AddButton(resp[all[1]], all[1]);
        AddButton(resp[all[2]], all[2]);
        AnimateButtons(true);
    }
    void AddButton(string text, int id)
    {
        TriviaButton b = Instantiate(button);
        b.transform.SetParent(container);
        b.transform.localScale = Vector3.one;
        b.Init(this, id, text);
        b.gameObject.SetActive(false);
        buttons.Add(b);
    }
    public void ButtonClicked(int id)
    {
        if (done)
            return;

        chronometer.Pause();
        done = true;
        Data.Instance.userData.SetAnswer(id);
        StartCoroutine( ResetTrivia(0.2f) );           
    }
    IEnumerator ResetTrivia(float delay)
    {
        yield return new WaitForSeconds(delay);
        AnimateButtons(false);
        Data.Instance.StartNextQuestion();
        yield return new WaitForSeconds(0.6f);
        content.GetComponent<Animation>().Play("trivia_off");       
        yield return new WaitForSeconds(0.5f);
        OnHideTrivia();        
    }
    void AnimateButtons(bool isOn)
    {
        StartCoroutine(AnimButtonsC(isOn));
    }
    IEnumerator AnimButtonsC(bool isOn)
    {
        foreach (TriviaButton b in buttons)
        {
            yield return new WaitForSeconds(0.2f);
            if(isOn)
                b.transform.gameObject.SetActive(true);
            else
            {
                b.AnimateOut();
            }
        }
        yield return null;
    }
}
