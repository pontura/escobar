using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaButton : MonoBehaviour
{
    Trivia trivia;
    public Text field;
    int id;
    public GameObject idle;
    public GameObject assetCorrect;
    public GameObject assetIncorrect;
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
    }

    public void Init(Trivia trivia, int id, string title )
    {
        if(btn==null)
            btn = GetComponent<Button>();
        btn.interactable = true ;
        idle.SetActive(true);
        assetCorrect.SetActive(false);
        assetIncorrect.SetActive(false);
        this.trivia = trivia;
        this.id = id;
        field.text = title;
    }

    public void Clicked()
    {
        GetComponent<Animation>().Play("buttonClicked");
        if (trivia.done)
            return;
        trivia.ButtonClicked(id);
        if(id==0)
            SetResult(true);
        else
            SetResult(false);
    }
    public void SetResult(bool correct)
    {
        Trivia trivia = UI.Instance.screensManager.all[2] as Trivia;
        if (trivia.type == Trivia.types.TRAINING)
        {
            Events.OnTrainingResponse(correct);
            idle.SetActive(false);
            if (correct)
            {
                assetCorrect.SetActive(true);
                Events.OnUIFX("si");
            }
            else
            {
                Events.OnUIFX("no");
                assetIncorrect.SetActive(true);
            }
        }
        else
        {
            Events.OnUIFX("si");
        }
    }
    public void AnimateOut()
    {
        GetComponent<Animation>().Play("button_off");
    }
}
