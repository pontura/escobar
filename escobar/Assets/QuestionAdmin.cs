using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAdmin : MainScreen
{
    public InputField pregunta;
    public InputField respuesta_bien;
    public InputField respuesta_1;
    public InputField respuesta_2;
    bool sended;

    public override void OnInit()
    {
        sended = false;
        if(Data.Instance.trainingData.activeQuestion != null)
        {
            TrainingData.Question question = Data.Instance.trainingData.activeQuestion;
            pregunta.text = question.pregunta;
            respuesta_bien.text = question.respuesta_bien;
            respuesta_1.text = question.respuesta_mal_1;
            respuesta_2.text = question.respuesta_mal_2;
        }
    }
    public void OnSubmit()
    {
        if (sended)
            return;

        sended = true;
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
            TrainingData.Question question = new TrainingData.Question();
            question.pregunta = pregunta.text;
            question.respuesta_bien = respuesta_bien.text;
            question.respuesta_mal_1 = respuesta_1.text;
            question.respuesta_mal_2 = respuesta_2.text;
            Data.Instance.serverManager.SendQuestion(question);
        }
    }
    public bool IsEmpty(InputField field)
    {
        if (field.text.Length < 1)
            return true;
        return false;
    }
}
