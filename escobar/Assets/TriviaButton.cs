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

    public void Init(Trivia trivia, int id, string title )
    {
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
        trivia.ButtonClicked(id);
        if(id==0)
            SetResult(true);
        else
            SetResult(false);
    }
    public void SetResult(bool correct)
    {
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
}
