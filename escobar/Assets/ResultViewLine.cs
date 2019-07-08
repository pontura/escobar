using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using FullSerializer;

public class ResultViewLine : MonoBehaviour
{
    public Text usernameField;
    public Text telField;
    public Text totalOKField;
    public Text timerField;
    public string uid;
    public int value;
    public float timer;

    public types type;
    public enum types
    {
        ALL,
        SINGLE
    }

    public void Init(string uid, types type, int value, float timer)
    {
        this.type = type;
        this.uid = uid;
        this.value = value;
        this.timer = timer;
        LoadUserData(uid);
        StartCoroutine(LoopUntilLoaded());
    }
    void LoadUserData(string uid)
    {
        UsersData.DataBasic data = Data.Instance.usersData.GetUserData(uid);
        if (data != null)
            OnUserBasicData(data);
        else
        {
            Events.OnUserBasicData += OnUserBasicData;
            Events.OnGetServerData("usuarios/" + uid, OnReady, "", 100000);            
        }
    }    
    void OnReady(object snapshot)
    {
        string url = Data.Instance.firebaseAuthManager.databaseURL + "/capitulos_participantes/" + uid + "/participantes.json?auth=" + Data.Instance.firebaseAuthManager.idToken;
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
            participantes.Reverse();
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
    UsersData.DataBasic data;
    void OnUserBasicData(UsersData.DataBasic _data)
    {
        if (uid == _data.uid)
        {
            this.data = _data;
        }
    }
    IEnumerator LoopUntilLoaded()
    {
        while (data == null)
            yield return null;

        usernameField.text = data.username;
        telField.text = data.tel;
        if (type == types.ALL)
            totalOKField.text = value.ToString();
        else
        {
            if (value == 0)
                totalOKField.text = "Bien";
            else
                totalOKField.text = "Mal";
        }
        timerField.text = timer.ToString();
        Events.OnUserBasicData -= OnUserBasicData;
    }
    void OnDestroy()
    {
        Events.OnUserBasicData -= OnUserBasicData;
    }
}
