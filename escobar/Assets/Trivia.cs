using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia : MainScreen
{
    public types type;
    public enum types
    {
        TORNEO,
        TRAINING
    }
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
    public GameObject videoPanel;
    float startTimerToResponse;
    public Text signalField;

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
    public override void OnEnabled()
    {
        timeOut.SetActive(false);

        if (type == types.TORNEO)
        {
            videoPanel.SetActive(true);
            signalField.text = "";
        }
        else
        {
            signalField.text = GetField();           
            videoPanel.SetActive(false);
        }
    }
    string GetField()
    {
        CapitulosData.Capitulo cap = Data.Instance.capitulosData.GetActual();

        if (cap == null)
        {
            return "Hoy no hay transmisión";
        }
        else if (Data.Instance.capitulosData.activeCapitulo.key == Data.Instance.userData.lastChapterPlayedKey)
        {
            string timeLive = Data.Instance.capitulosData.activeCapitulo.time;

            if (Data.Instance.dateData.dateTime.Hour.ToString() == timeLive)
            {
                return "¡Hay transmisión en vivo ahora!";
            }
            else
            {
                if (int.Parse(Data.Instance.capitulosData.activeCapitulo.time) < Data.Instance.dateData.dateTime.Hour)
                {
                    return "La transmisión finalizó.";
                }
                else
                {
                    return "La próxima transmisión es a las " + Data.Instance.capitulosData.activeCapitulo.time + " hs.";
                }
            }
        }
        return "";
    }
    public override void OnInit()
    {
        if (type == types.TORNEO)
        {
            Events.OnNewQuestion(Data.Instance.triviaData.GetActualQuestion());
        } else { 
            Events.OnNewTrainingQuestion(Data.Instance.trainingData.GetActualQuestion());
        }
    }
    void OnTriviaTimeOut()
    {
        if (done)
            return;

        done = true;
        DelayToPreload();
        chronometer.Pause();
        done = true;
        if (type == types.TORNEO)
        {
            float duration = Time.time - startTimerToResponse;
            Data.Instance.userData.SetAnswer(-1, duration);
        }
        StartCoroutine(ResetTrivia(0.2f));
    }
    void TimeOutDelayed()
    {
        timeOut.SetActive(true);
    }
    void OnShowTrivia()
    {
        startTimerToResponse = Time.time;
        buttons.Clear();
        done = false;
        content.SetActive(true);
        all = new int[] { 0, 1, 2 };
        Utils.ShuffleListNums(all);
        Utils.RemoveAllChildsIn(container);
        LoadData();
        chronometer.Init(10);
       // Invoke("DelayToPreload", 0.5f);
    }
    void DelayToPreload()
    {
        PlaylistData.Sources[] s = Data.Instance.triviaData.GetNextVideoSource();
        if (s != null)
        {
            Events.OnPreLoadVideo(s);
        }
    }
    void OnHideTrivia()
    {        
        content.SetActive(false);
    }
    void LoadData()
    {
        if (type == types.TORNEO)
            LoadDataFromVideoData(Data.Instance.triviaData.GetActualQuestion());
        else
            LoadDataFromTraining(Data.Instance.trainingData.GetActualQuestion());
    }
    void LoadDataFromVideoData(PlaylistData.VideoData dataQuestion)
    {
        field.text = dataQuestion.title;
        string[] resp = Data.Instance.triviaData.GetAnswwers();

        for (int a = 0; a < 3; a++)
        {
            string r = resp[all[a]];
            AddButton(r, all[a]);
        }

        AnimateButtons(true);
    }
    void LoadDataFromTraining(TrainingData.Question dataQuestion)
    {
        field.text = dataQuestion.pregunta;
        string[] resp = Data.Instance.trainingData.GetAnswwers();

        for (int a = 0; a < 3; a++)
        {
            string r = resp[all[a]];
            AddButton(r, all[a]);
        }

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

        DelayToPreload();

        chronometer.Pause();
        done = true;

        if (type == types.TORNEO)
        {
            float duration = Time.time - startTimerToResponse;
            Data.Instance.userData.SetAnswer(id, duration);
        }

        StartCoroutine( ResetTrivia(0.2f) );           
    }
    IEnumerator ResetTrivia(float delay)
    {
        yield return new WaitForSeconds(delay);
        AnimateButtons(false);

        if(type == types.TORNEO)
            Data.Instance.StartNextQuestion();

        yield return new WaitForSeconds(0.6f);
        content.GetComponent<Animation>().Play("trivia_off");       
        yield return new WaitForSeconds(0.5f);
        OnHideTrivia();
        if (type == types.TRAINING)
        {
            yield return new WaitForSeconds(2);
            Events.OnNewTrainingQuestion(Data.Instance.trainingData.GetActualQuestion());
        }
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
