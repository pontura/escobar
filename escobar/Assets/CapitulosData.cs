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
        public string key;
        public string date;
        public string time;
        public string playlistID;
    }
    public int capituloID = 0;
    public Capitulo activeCapitulo;

    void Start()
    {
        //PONTURA: cambiar a futuro...
        Invoke("LoadData", 1);
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
        return capitulos[capituloID];
    }
    void OnReady(DataSnapshot snapshot)
    {
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
        foreach (Capitulo c in capitulos)
            return c;
        return null;
    }
}
