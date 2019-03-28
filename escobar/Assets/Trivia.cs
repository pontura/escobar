using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia : ScreenBase
{
    public Text field;
    public TriviaButton button;
    public Transform container;
    int buttonId = 0;
    public Chronometer chronometer;
    int[] all;

    public override void OnInit()
    {
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
        Data.Instance.userData.SetAnswer(id);
        UI.Instance.screensManager.Reset();
        Data.Instance.StartNextQuestion();
    }
}
