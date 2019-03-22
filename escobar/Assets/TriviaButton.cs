using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaButton : MonoBehaviour
{
    Trivia trivia;
    public Text field;
    int id;
    public void Init(Trivia trivia, int id, string title )
    {
        this.trivia = trivia;
        this.id = id;
        field.text = title;
    }
    public void Clicked()
    {
        trivia.ButtonClicked(id);
    }
}
