using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class CapitulosData : MonoBehaviour
{
    public List<Capitulo> capitulos;

    [Serializable]
    public class Capitulo
    {
        public string uid;
        public string key;
        public string date;
        public string time;
        public string playlistID;
    }
    public int capituloID = 0;
    public Capitulo activeCapitulo;

    void Start()
    {
        LoopTillFirebaseReady();
    }
    void LoopTillFirebaseReady()
    {
        if(!Data.Instance.serverManager.isDone)
            Invoke("LoopTillFirebaseReady", 0.25f);
        else
            OnRefreshCapitulos();
    }
    void LoadData()
    {
        Events.OnGetServerData("capitulos", OnReady);
    }
   public void OnRefreshCapitulos()
    {
        capitulos.Clear();
        LoadData();
        activeCapitulo = null;
    }
    public Capitulo GetActual()
    {
        string today = Data.Instance.dateData.GetTodayParsed();
        foreach (Capitulo c in capitulos)
        {
            if (c.date == today)
            {
                activeCapitulo = c;
                return capitulos[capituloID];
            }
        }
        return null;        
    }
    void OnReady(DataSnapshot snapshot)
    {
        capitulos.Clear();
        foreach (DataSnapshot data in snapshot.Children)
        {
            Capitulo tData = new Capitulo();
            IDictionary dataDictiontary = (IDictionary)data.Value;
            tData.key = data.Key;
            tData.date = dataDictiontary["date"].ToString();
            tData.time = dataDictiontary["time"].ToString();
            tData.playlistID = dataDictiontary["playlistID"].ToString();

            capitulos.Add(tData);
        }
    }
    public Capitulo GetNext()
    {
        string today = Data.Instance.dateData.GetTodayParsed();
        foreach (Capitulo c in capitulos)
        {
            if (c.date == today)
            {
                if (capituloID+1 == capitulos.Count)
                    return null;
                else  return capitulos[capituloID + 1];
            }
        }
        return null;
    }
}
