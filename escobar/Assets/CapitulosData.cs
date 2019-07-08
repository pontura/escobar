using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proyecto26;
using FullSerializer;

public class CapitulosData : MonoBehaviour
{
    public List<Capitulo> capitulos;

    [Serializable]
    public class Capitulo
    {
        public string ts;
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
        if (!Data.Instance.firebaseAuthManager.isDone)
        {
            Invoke("LoopTillFirebaseReady", 0.25f);
        } else if (!UI.Instance.screensManager.isAdmin)
            OnRefreshCapitulos();
    }
    void LoadData()
    {
        string url = Data.Instance.firebaseAuthManager.databaseURL + "/capitulos.json?auth=" + Data.Instance.firebaseAuthManager.idToken;
        print("_____" + url);

        RestClient.Get(url).Then(response =>
        {
            //   string username = user.username;
            fsSerializer serializer = new fsSerializer();
            fsData userData = fsJsonParser.Parse(response.Text);
            Dictionary<string, CapitulosData.Capitulo> caps = null;
            serializer.TryDeserialize(userData, ref caps);
           
            foreach (CapitulosData.Capitulo cap in caps.Values)
                capitulos.Add( cap);

            int id = 0;
            foreach (string d in caps.Keys)
            {
                capitulos[id].key = d;
                id++;
            }
        }).Catch(error =>
        {
            Debug.Log(error);
        });
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
           // print(c.date + "          today : " + today);
            if (c.date == today)
            {
                activeCapitulo = c;
                return activeCapitulo;
            }
        }
        return null;        
    }
    void OnReady(List<CapitulosData.Capitulo> capitulos)
    {
        foreach(Capitulo c in capitulos)
            capitulos.Add(c);
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
