using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitulosAdmin : MainScreen
{
    public AdminButton adminButton;
    public Transform container;

    public List<AdminButton> all;

    public override void OnEnabled()
    {
        Data.Instance.capitulosData.OnRefreshCapitulos();
        Load();
    }
    void Load()
    {
        if(Data.Instance.capitulosData.capitulos.Count == 0)
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

            AdminButton button = Instantiate(adminButton);
            button.transform.SetParent(container);
            button.transform.localScale = Vector3.one;
            button.Init(id, c.date + " " + c.time + "hs", OnClicked);
            id++;
            all.Add(button);
        }
    }
    void OnClicked(AdminButton button)
    {
        print("CapitulosAdmin clicked" + button.id);
        Data.Instance.capitulosData.activeCapitulo = Data.Instance.capitulosData.capitulos[button.id];
        Data.Instance.triviaData.LoadPlaylist(Data.Instance.capitulosData.activeCapitulo.playlistID, GotoEdit);
    }
    public void AddNew()
    {        
        GotoEdit();
    }
    void GotoEdit()
    {
        UI.Instance.screensManager.LoadScreen(4, true);
    }

}
