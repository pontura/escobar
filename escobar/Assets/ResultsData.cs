using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class ResultsData : MonoBehaviour
{

    public List<Participante> participantes;

    [Serializable]
    public class Participante
    {
        public string uid;
        public List<Results> respuestas;
        public float totalTimeCorrect;
        public int totalCorrect;
    }
    [Serializable]
    public class Results
    {
        public int respuesta;
        public float timer;
    }
    public CapitulosData.Capitulo activeCapitulo;

    void Start()
    {
        LoopToLoadCapitulos();
    }
    void LoopToLoadCapitulos()
    {
        if (Data.Instance.capitulosData.capitulos.Count == 0)
        {
            Invoke("LoopToLoadCapitulos", 1);
        }
        else if (UI.Instance.screensManager.isAdmin)
            OnRefreshParticipantes();
    }   
    public void OnRefreshParticipantes()
    {
        participantes.Clear();
        LoadData("-LinLrfHJmmZVpD_ITq1");
    }
    void LoadData(string uid)
    {
        string url = "capitulos_participantes/" + uid + "/participantes";
        Events.OnGetServerData(url, OnReady);
        print("LoadData url: " + url);
    }
    void OnReady(DataSnapshot snapshot)
    {
        print("OnReady snapshot: " + snapshot);
        participantes.Clear();
        foreach (DataSnapshot data in snapshot.Children)
        {
            Participante tData = new Participante();
            IDictionary dataDictiontary = (IDictionary)data.Value;
            tData.uid = data.Key;
            tData.respuestas = new List<Results>();
            
            List<object> all = (List<object>)dataDictiontary["respuestas"];

            int totalCorrect = 0;
            float totalTimeCorrect = 0;
            foreach(object o in all)
            {
                Results result = new Results();
                IDictionary d = (IDictionary)o;

                string respuesta = d["respuesta"].ToString();
                string timer = d["timer"].ToString();

                result.respuesta = int.Parse(respuesta);
                result.timer = float.Parse(timer);
                tData.respuestas.Add(result);

                if (result.respuesta == 0)
                {
                    totalTimeCorrect += result.timer;
                    totalCorrect++;
                }
                    
            }
            tData.totalCorrect = totalCorrect;
            tData.totalTimeCorrect = totalTimeCorrect;
            participantes.Add(tData);
        }
    }
   
}
