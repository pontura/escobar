using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapituloEdit : MainScreen
{
    public GameObject deleteButton;
    public InputField dateField;
    public InputField timeField;
    public InputField playlistIDField;
    bool sended;

    public override void OnEnabled()
    {
        sended = false;
        if (Data.Instance.triviaData.data != null)
        {
            deleteButton.SetActive(true);       
        }
        else
        {
            deleteButton.SetActive(false);
        }

        PlaylistData data = Data.Instance.triviaData.data;
        dateField.text = data.date;
        timeField.text = data.time;
        playlistIDField.text = data.playlistID;
    }
    public void OnSubmit()
    {
        if (IsEmpty(dateField))
            Events.OnTooltip("Indica una fecha: (ej: 22/01/2019)", dateField.transform);
        else if (IsEmpty(timeField))
            Events.OnTooltip("Indica una hora: (ej: 19hs)", timeField.transform);
        else if (IsEmpty(playlistIDField))
            Events.OnTooltip("Agregá el playlistID de JWPlayer", playlistIDField.transform);
        else
        {
            if (sended)
                return;

            sended = true;

            PlaylistData d = new PlaylistData();
            d.date = dateField.text;
            d.time = timeField.text;
            d.playlistID = playlistIDField.text;
            
            if (Data.Instance.triviaData.data != null && Data.Instance.triviaData.data.playlistID != "")
                Data.Instance.serverManager.UpdateData("capitulos", Data.Instance.capitulosData.activeCapitulo.key, d);
            else
                Data.Instance.serverManager.PushData("capitulos", d);

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
        Data.Instance.serverManager.DeleteQuestion(activeQuestion.key);

        Events.OnRefreshTrainingData();

    }
    public override void OnBack()
    {
        Data.Instance.trainingData.activeQuestion = null;
    }
}
