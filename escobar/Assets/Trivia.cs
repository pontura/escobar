using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia: ScreenBase
{
    public Text field;
    public TriviaButton button;
    public Transform container;
    int buttonId = 0;

    public override void OnInit()
    {
        Utils.RemoveAllChildsIn(container);
        LoadData(Data.Instance.GetActualQuestion());
    }
    void LoadData(Data.Question dataQuestion)
    {
        field.text = dataQuestion.question;        
        AddButton(dataQuestion.answer1);
        AddButton(dataQuestion.answer2);
        AddButton(dataQuestion.answer3);
    }
    void AddButton(string text)
    {
        TriviaButton b = Instantiate(button);
        b.transform.SetParent(container);
        b.transform.localScale = Vector3.one;
        b.Init(this, buttonId, text);
        buttonId++;
    }
    public void ButtonClicked(int id)
    {
        UI.Instance.screensManager.Reset();
        Data.Instance.StartNextQuestion();
    }
}
