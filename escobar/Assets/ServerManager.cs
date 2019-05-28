using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

public class ServerManager : MonoBehaviour
{
    int diferenciaHoraria = 243;

    DatabaseReference reference;
    Firebase.FirebaseApp app;

    void Start()
    {
       // print("Horario local (internet): " + GetTimeNist());
        Events.OnGetServerData += OnGetServerData;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://triviaescobar.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                print("App ready");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public DateTime GetTimeNist()
    {
        DateTime dateTime = DateTime.MinValue;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
        request.Method = "GET";
        request.Accept = "text/html, application/xhtml+xml, */*";
        request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
        request.ContentType = "application/x-www-form-urlencoded";
        request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore); //No caching
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            StreamReader stream = new StreamReader(response.GetResponseStream());
            string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
            string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
            double milliseconds = Convert.ToInt64(time) / 1000.0;
            milliseconds += diferenciaHoraria * (60 * 1000);
            dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
        }

        return dateTime;
    }
    void OnDestroy()
    {
        Events.OnGetServerData -= OnGetServerData;
    }
    

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["capitulo"] = "Capitulo 1";
        result["id"] = "ASDASD";
        result["nombre"] = "Pontura";
        result["telefono"] = "123423423423";
        result["email"] = "pontura@gmail.com";
        int[] respuestas = new int[2];
        respuestas[0] = 1;
        respuestas[1] = 1;
        result["respuestas"] = respuestas;
        return result;
    }

    public void Send()
    {
        TriviaData data = new TriviaData();
        data.capitulo = 1;
        UserData userData = Data.Instance.userData;
        data.id = userData.deviceID;
        data.nombre = userData.username;
        data.telefono = userData.tel;
        //data.email = "pontura@gmail.com";
        //int[] respuestas = new int[2];
        //respuestas[0] = 1;
        //respuestas[1] = 1;
        data.respuestas = userData.answers;

        string json = JsonUtility.ToJson(data);
        reference.Child("usuarios").Push().SetRawJsonValueAsync(json);
        print("Sended " + json);
    }

  

    public void OnGetServerData(string childName, System.Action<DataSnapshot> OnReady)
    {
        FirebaseDatabase.DefaultInstance
       .GetReference(childName)
       .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
               OnReady(snapshot);
           }
        }
        );
    }
    public void UpdateData(string table, string key, object obj)
    {
        string json = JsonUtility.ToJson(obj);
        reference.Child(table).Child(key).SetRawJsonValueAsync(json);
        print("UpdateQuestion " + json);    
    }
    public void PushData(string table, object obj)
    {
        string json = JsonUtility.ToJson(obj);
        reference.Child(table).Push().SetRawJsonValueAsync(json);
        print("PushQuestion " + json);
    }
    public void DeleteQuestion(string key)
    {
        reference.Child("entrenamiento").Child(key).RemoveValueAsync();
        print("DeleteQuestion " + key);
    }
}
