using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingAdmin : MainScreen
{
    public AdminButton adminButton;
    public Transform container;

    public override void OnEnabled()
    {
        Data.Instance.trainingData.OnRefreshTrainingData();
        Load();
    }
    void Load()
    {
        if (Data.Instance.trainingData.entrenamiento.Count == 0)
        {
            Invoke("Load", 0.2f);
        }
        else
        {
            DrawData();
        }
    }
    void DrawData()
    {
        CancelInvoke();
        int id = 0;
        Utils.RemoveAllChildsIn(container);
        foreach (TrainingData.Training q in Data.Instance.trainingData.entrenamiento)
        {
            
            AdminButton button = Instantiate(adminButton);
            button.transform.SetParent(container);
            button.transform.localScale = Vector3.one;
            button.Init(id, q.preguntas.pregunta, OnClicked);
            id++;
        }
    }
    void OnClicked(AdminButton button)
    {
        Data.Instance.trainingData.activeQuestion = Data.Instance.trainingData.entrenamiento[button.id];
        GotoEdit();
    }
    public void AddNewQuestion()
    {
        GotoEdit();
    }
    void GotoEdit()
    {
        UI.Instance.screensManager.LoadScreen(2, true);
    }

}
