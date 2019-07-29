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
    public Text edadField;

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
    //solo para mostrar usuarios
    public void InitOnlyUserData(UsersData.DataBasic data)
    {
        usernameField.text = data.username;
        telField.text = data.tel;
        edadField.text = data.edad;
    }
    void LoadUserData(string uid)
    {
        UsersData.DataBasic data = Data.Instance.usersData.GetUserData(uid);
        if (data != null)
            OnUserBasicData(data);
        else
        {
            Events.OnUserBasicData += OnUserBasicData;
            LoadNewUserData();
        }
    }    
    void LoadNewUserData()
    {
        //UsersData.DataBasic tData = new UsersData.DataBasic();
        //IDictionary dataDictiontary = (IDictionary)snapshot.Value;
        //tData.uid = dataDictiontary["uid"].ToString();
        //tData.tel = dataDictiontary["tel"].ToString();
        //tData.username = dataDictiontary["username"].ToString();
        //Events.OnUserBasicData(tData);

        string url = Data.Instance.firebaseAuthManager.databaseURL + "/usuarios.json?auth=" + Data.Instance.userData.token;
        print("LoadResultsData _____" + url);

        RestClient.Get(url).Then(response =>
        {
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(response.Text);
            Dictionary<string, UsersData.DataBasic> results = null;
            serializer.TryDeserialize(data, ref results);
            foreach (UsersData.DataBasic d in results.Values)
            {
                if(uid == d.uid)
                {
                    Events.OnUserBasicData(d);
                }
            }
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
        edadField.text = data.edad;

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
