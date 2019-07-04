using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

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
    }
    void LoadUserData(string uid)
    {
        UsersData.DataBasic data = Data.Instance.usersData.GetUserData(uid);
        if (data != null)
            OnUserBasicData(data);
        else
        {
            Events.OnUserBasicData += OnUserBasicData;
            Events.OnGetServerData("usuarios/" + uid, OnReady);            
        }
    }    
    void OnReady(DataSnapshot snapshot)
    {        
        UsersData.DataBasic tData = new UsersData.DataBasic();
        IDictionary dataDictiontary = (IDictionary)snapshot.Value;
        tData.uid = snapshot.Key;
        tData.tel = dataDictiontary["tel"].ToString();
        tData.username = dataDictiontary["username"].ToString();
        Events.OnUserBasicData(tData);
        OnUserBasicData(tData);
    }
    void OnUserBasicData(UsersData.DataBasic data)
    {
        if (uid == data.uid)
        {
            usernameField.text = data.username;
            telField.text = data.tel;

            if(type == types.ALL)
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
    }
    void OnDestroy()
    {
        Events.OnUserBasicData -= OnUserBasicData;
    }
}
