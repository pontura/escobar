using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsView : MainScreen
{
    public ResultViewLine line;
    public Transform container;
    public Text capituloTitle;
    public Text totalParticipantes;
    public Dropdown dropDown;
    public List<string> dropDownContent;

    public override void OnEnabled()
    {
        Utils.RemoveAllChildsIn(container);
        LoopUntilDataLoaded();
        AddDropdownOptions();
        capituloTitle.text = Data.Instance.capitulosData.activeCapitulo.date.ToString();
    } 
    void AddDropdownOptions()
    {
        dropDownContent.Clear();
        dropDown.ClearOptions();
        List<Dropdown.OptionData> allDropOptions = new List<Dropdown.OptionData>();

        dropDownContent.Add("Resultado final (todas las preguntas)");
        foreach (PlaylistData.VideoData data in Data.Instance.triviaData.data.playlist)
            dropDownContent.Add(data.title);

        foreach(string content in dropDownContent)
            allDropOptions.Add(new Dropdown.OptionData(content));

        dropDown.AddOptions(allDropOptions);
    }
    public void OnDropdownValueChange()
    {
        print(dropDown.value);
        if (dropDown.value > 0)
            LoadDataForQuestion(dropDown.value);
        else
            LoadTotalData();
    }
    public override void OnBack()
    {
        dropDown.SetValueWithoutNotify(0);
    }
    void LoopUntilDataLoaded()
    {
        if (Data.Instance.resultsData.participantes.Count > 0)
            LoadTotalData();
        else
            Invoke("LoopUntilDataLoaded", 0.25f);
    }
    void LoadTotalData()
    {
        Utils.RemoveAllChildsIn(container);
        foreach (ResultsData.Participante data in Data.Instance.resultsData.participantes)
        {
            ResultViewLine newLine = Instantiate(line);
            newLine.transform.SetParent(container);
            string uid = data.uid;
            int correctas = data.totalCorrect;
            float timer = data.totalTimeCorrect;
            newLine.Init(uid, ResultViewLine.types.ALL, correctas, timer);
            newLine.transform.transform.localScale = Vector3.one;
        }
    }
    void LoadDataForQuestion( int questionID )
    {
        Utils.RemoveAllChildsIn(container);
        foreach (ResultsData.Participante data in Data.Instance.resultsData.participantes)
        {
            ResultViewLine newLine = Instantiate(line);
            newLine.transform.SetParent(container);
            string uid = data.uid;
            int respuesta = data.respuestas[questionID-1].respuesta;
            float timer = data.respuestas[questionID - 1].timer;
            newLine.Init(uid, ResultViewLine.types.SINGLE,  respuesta, timer);
            newLine.transform.transform.localScale = Vector3.one;
        }
    }
}
