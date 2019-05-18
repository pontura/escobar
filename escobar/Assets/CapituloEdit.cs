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
        if (Data.Instance.capitulosData.activeCapitulo != null && Data.Instance.capitulosData.activeCapitulo.key != "")
        {
            deleteButton.SetActive(true);
            LoadTrivia();
            CapitulosData.Capitulo data = Data.Instance.capitulosData.activeCapitulo;
            dateField.text = data.date;
            timeField.text = data.time;
            playlistIDField.text = data.playlistID;
        }
        else
        {
            deleteButton.SetActive(false);
        }
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

            CapitulosData.Capitulo d = new CapitulosData.Capitulo();
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


    public QuestionLine qline;
    public Transform container;
    public List<AdminButton> all;

    void LoadTrivia()
    {
        Utils.RemoveAllChildsIn(container);
        PlaylistData data = Data.Instance.triviaData.data;
        foreach(PlaylistData.VideoData d in data.playlist)
        {
            QuestionLine line = Instantiate(qline);
            line.transform.SetParent(container);
            line.Init(d, null);
            line.transform.transform.localScale = Vector3.one;
        }
    }
    
    public void EditPlaylist()
    {
        Data.Instance.triviaData.Open_URL(Data.Instance.capitulosData.activeCapitulo.playlistID);
    }
}
