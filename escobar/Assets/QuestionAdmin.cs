using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAdmin : MainScreen
{
    public GameObject deleteButton;
    public InputField pregunta;
    public InputField respuesta_bien;
    public InputField respuesta_1;
    public InputField respuesta_2;
    bool sended;

    public override void OnEnabled  ()
    {
        sended = false;
        if(Data.Instance.trainingData.activeQuestion != null 
            && Data.Instance.trainingData.activeQuestion.key != null
            && Data.Instance.trainingData.activeQuestion.key.Length>0)
        {
            deleteButton.SetActive(true);
            TrainingData.Question question = Data.Instance.trainingData.activeQuestion.preguntas;
            pregunta.text = question.pregunta;
            respuesta_bien.text = question.respuesta_bien;
            respuesta_1.text = question.respuesta_mal_1;
            respuesta_2.text = question.respuesta_mal_2;
        }
        else
        {
            pregunta.text = "";
            respuesta_bien.text = "";
            respuesta_1.text = "";
            respuesta_2.text = "";
            deleteButton.SetActive(false);
        }
    }
    public void OnSubmit()
    {
    
        string tooltipText = "No puede estar vacío este campo";
        if (IsEmpty(pregunta))
            Events.OnTooltip(tooltipText, pregunta.transform);
        else if (IsEmpty(respuesta_bien))
            Events.OnTooltip(tooltipText, respuesta_bien.transform);
        else if (IsEmpty(respuesta_1))
            Events.OnTooltip(tooltipText, respuesta_1.transform);
        else if (IsEmpty(respuesta_2))
            Events.OnTooltip(tooltipText, respuesta_2.transform);
        else
        {
            if (sended)
                return;

            sended = true;

            TrainingData.Question question = new TrainingData.Question();
            question.pregunta = pregunta.text;
            question.respuesta_bien = respuesta_bien.text;
            question.respuesta_mal_1 = respuesta_1.text;
            question.respuesta_mal_2 = respuesta_2.text;
            TrainingData.Training activeQuestion = Data.Instance.trainingData.activeQuestion;

            if(activeQuestion != null && activeQuestion.key != null)
                Data.Instance.firebaseAuthManager.SaveTraining(question, activeQuestion.key);
            else
                Data.Instance.firebaseAuthManager.SaveTraining(question);

            Events.OnRefreshTrainingData();

            Invoke("Back", 1);
        }
    }
    public bool IsEmpty(InputField field)
    {
        if (field.text.Length < 1)
            return true;
        return false;
    }
    public void Delete()
    {
        if (sended)
            return;

        sended = true;

        TrainingData.Training activeQuestion = Data.Instance.trainingData.activeQuestion;
        Data.Instance.firebaseAuthManager.DeleteTraining(activeQuestion.key);

        Back();

    }
    public override void OnBack()
    {
        Data.Instance.trainingData.activeQuestion = null;
    }
}
