using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Proyecto26;
using FullSerializer;
using System.Linq;

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
        public int score;
    }
    [Serializable]
    public class Results
    {
        public int respuesta;
        public float timer;
    }
    public CapitulosData.Capitulo activeCapitulo;

    public void LoadResultsData(string uid)
    {
        participantes.Clear();
        //string url = "capitulos_participantes/" + uid + "/participantes";
        //Events.OnGetServerData(url, OnReady, "score", 100);
        //print("LoadData url: " + url);

        string url = Data.Instance.firebaseAuthManager.databaseURL + "/capitulos_participantes/" + uid + "/participantes.json?orderBy=\"score\"&limitToLast=3&auth=" + Data.Instance.userData.token;
        print("LoadResultsData _____" + url);

        RestClient.Get(url).Then(response =>
        {
            //   string username = user.username;
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(response.Text);
            Dictionary<string, Participante> results = null;
            serializer.TryDeserialize(data, ref results);

            foreach (Participante d in results.Values)
            {
               // print("score: " + d.score);
                int totalCorrect = 0;
                float totalTimeCorrect = 0;
                foreach (Results r in d.respuestas)
                {
                    if (r.respuesta == 0)
                    {
                        totalTimeCorrect += r.timer;
                        totalCorrect++;
                    }
                        
                }
                d.totalCorrect = totalCorrect;
                d.totalTimeCorrect = totalTimeCorrect;
                participantes.Add(d);
            }
            participantes = GetOrderByScoreGeneral();

            //foreach (Participante d in participantes)
            //{
            //    print("_________score2: " + d.score);
            //}
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
    public List<Participante> GetOrderByScoreGeneral()
    {
        participantes = participantes.OrderBy(value => value.score).ToList();
        participantes.Reverse();
        return participantes;
    }
    public List<Participante> GetOrderByQuestionScore(int questionID)
    {
        participantes = participantes.OrderBy(value => value.respuestas[questionID].timer).ToList();
        return participantes;
    }
    void OnReady(object snapshot)
    {
        participantes.Clear();
        //foreach (DataSnapshot data in snapshot.Children)
        //{
        //    Participante tData = new Participante();
        //    IDictionary dataDictiontary = (IDictionary)data.Value;
        //    tData.uid = data.Key;
        //    tData.score = int.Parse(dataDictiontary["score"].ToString());
        //    tData.respuestas = new List<Results>();
            
        //    List<object> all = (List<object>)dataDictiontary["respuestas"];

        //    int totalCorrect = 0;
        //    float totalTimeCorrect = 0;
        //    foreach(object o in all)
        //    {
        //        Results result = new Results();
        //        IDictionary d = (IDictionary)o;

        //        string respuesta = d["respuesta"].ToString();
        //        string timer = d["timer"].ToString();

        //        result.respuesta = int.Parse(respuesta);
        //        result.timer = float.Parse(timer);
        //        tData.respuestas.Add(result);

        //        if (result.respuesta == 0)
        //        {
        //            totalTimeCorrect += result.timer;
        //            totalCorrect++;
        //        }
                    
        //    }
        //    tData.totalCorrect = totalCorrect;
        //    tData.totalTimeCorrect = totalTimeCorrect;
        //    participantes.Add(tData);
        //}
        participantes.Reverse();
    }



   
}
