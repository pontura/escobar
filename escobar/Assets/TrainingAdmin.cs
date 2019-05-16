using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingAdmin : MainScreen
{
    public AdminButton adminButton;
    public Transform container;

    public override void OnInit()
    {
        Utils.RemoveAllChildsIn(container);
        foreach (TrainingData.Question q in Data.Instance.trainingData.preguntas)
        {
            
            AdminButton button = Instantiate(adminButton);
            button.transform.SetParent(container);
            button.transform.localScale = Vector3.one;
            button.Init(q, OnClicked);
        }
    }
    void OnClicked(AdminButton button)
    {
        print(button.questionData.pregunta);
    }
}
