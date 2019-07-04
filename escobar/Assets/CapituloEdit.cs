using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapituloEdit : MainScreen
{
    public GameObject editButton;
    public GameObject deleteButton;
    public DatePanel dateField;
    public InputField timeField;
    public InputField playlistIDField;
    public Tutorial tutorial;
    bool sended;
    bool editing;
    public override void OnEnabled()
    {
        Utils.RemoveAllChildsIn(container);
        sended = false;

        if (Data.Instance.capitulosData.activeCapitulo != null && Data.Instance.capitulosData.activeCapitulo.key != null)
        {
            editing = true;
            deleteButton.SetActive(true);
            editButton.SetActive(true);
            LoadTrivia();
            CapitulosData.Capitulo data = Data.Instance.capitulosData.activeCapitulo;
            dateField.Init(data.date);
            timeField.text = data.time;
            playlistIDField.text = data.playlistID;
            tutorial.panel.gameObject.SetActive(false);
        }
        else
        {
            editing = false;
            string today = System.DateTime.Now.Day + "/" + "Feb" + "/" + System.DateTime.Now.Year;
            dateField.Init(today);
            dateField.InitMonths(Data.Instance.dateData.monthList[System.DateTime.Now.Month-1]);
            deleteButton.SetActive(false);
            editButton.SetActive(false);
            timeField.text = "";
            playlistIDField.text = "";
            tutorial.panel.gameObject.SetActive(true);
        }
    }
    public void OnSubmit()
    {
        if (dateField.GetValue() == "")
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
            d.uid = Data.Instance.userData.uid;
            d.date = dateField.GetValue();
            d.time = timeField.text;
            d.ts = dateField.GetTimestamp();
            d.playlistID = playlistIDField.text;


            if (editing)
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
        
        PlaylistData data = Data.Instance.triviaData.data;
        foreach(PlaylistData.VideoData d in data.playlist)
        {
            QuestionLine line = Instantiate(qline);
            line.transform.SetParent(container);
            line.Init(d, OnClicked);
            line.transform.transform.localScale = Vector3.one;
        }
    }
    public void OnClicked(PlaylistData.VideoData data)
    {
        Data.Instance.triviaData.OPEN_VIDEO_EDIT(data.mediaid);
    }
    public void EditPlaylist()
    {
        Data.Instance.triviaData.Open_URL(Data.Instance.capitulosData.activeCapitulo.playlistID);
    }
}
