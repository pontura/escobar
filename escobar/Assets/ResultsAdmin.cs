using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsAdmin : MainScreen
{
    public AdminButton adminButton;
    public Transform container;

    public GameObject signal;
    public Text SignalField;

    public Text otherField;
   

    public states state;
    public enum states
    {
        ONLY_ACTIVE,
        ALL
    }

    public List<AdminButton> all;

    public override void OnEnabled()
    {
        Utils.RemoveAllChildsIn(container);
        Data.Instance.capitulosData.OnRefreshCapitulos();
        state = states.ONLY_ACTIVE;
        SetOtherButton();
        Load();
        signal.SetActive(true);
        SetSignal("Cargando capítulos...");
    }
    void SetSignal(string data)
    {
        SignalField.text = data;
        if (data.Length>0)
            signal.SetActive(true);
        else
            signal.SetActive(false);
    }
    void Load()
    {
        if (Data.Instance.capitulosData.capitulos.Count == 0)
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
        Data.Instance.capitulosData.activeCapitulo = null;
        int id = 0;
        all.Clear();
        Utils.RemoveAllChildsIn(container);
        foreach (CapitulosData.Capitulo c in Data.Instance.capitulosData.capitulos)
        {
            if (
                (state == states.ONLY_ACTIVE && !Data.Instance.dateData.IsFromThePast(c.date))
                ||
                state == states.ALL
                )
            {
                AdminButton button = Instantiate(adminButton);
                button.transform.SetParent(container);
                button.transform.localScale = Vector3.one;
                button.Init(id, c.date + " " + c.time + "hs", OnClicked);
                all.Add(button);
            }
            id++;
        }
        if (all.Count == 0)
            SetSignal("No hay capítulos...");
        else
            SetSignal("");
    }
    void OnClicked(AdminButton button)
    {        
        Data.Instance.capitulosData.activeCapitulo = Data.Instance.capitulosData.capitulos[button.id];
        Data.Instance.resultsData.LoadResultsData(Data.Instance.capitulosData.activeCapitulo.key);
        Data.Instance.triviaData.LoadPlaylist(Data.Instance.capitulosData.activeCapitulo.playlistID, GotoEdit, true);
    }
    void GotoEdit()
    {
        UI.Instance.screensManager.LoadScreen(6, true);
    }
    public void OthersClicked()
    {
        if (state == states.ONLY_ACTIVE)
        {
            state = states.ALL;
        }
        else
        {
            state = states.ONLY_ACTIVE;
        }
        SetOtherButton();
        DrawData();
    }
    void SetOtherButton()
    {
        if (state == states.ONLY_ACTIVE)
            otherField.text = "VER TODOS";
        else
            otherField.text = "SOLO ACTIVOS";
    }
}
