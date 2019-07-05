using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserData : MonoBehaviour
{
    public string lastChapterPlayedKey;
   // public string uid;
   // public string username;
    //public string tel;
   // public string deviceID;
    public List<AnswersData> answers;
    public UserDataInDatabase userDataInDatabase;

    [Serializable]
    public class UserDataInDatabase
    {
        public string username;
        public string tel;
        public string uid;
        public string deviceID;
    }

    [Serializable]
    public class AnswersData
    {
        public int respuesta;
        public float timer;
    }

    void Awake()
    {
        lastChapterPlayedKey = PlayerPrefs.GetString("lastChapterPlayedKey", "");
        userDataInDatabase.username = PlayerPrefs.GetString("username", "");
        userDataInDatabase.tel = PlayerPrefs.GetString("tel", "");
        userDataInDatabase.deviceID = PlayerPrefs.GetString("deviceID", "");
        userDataInDatabase.uid = PlayerPrefs.GetString("uid", "");
    }
    public bool IsLogged()
    {
        if (userDataInDatabase.deviceID.Length<2)
            return false;
        return true;
    }
    public void SaveUser(string username, string tel)
    {
        userDataInDatabase.deviceID = SystemInfo.deviceUniqueIdentifier;
        userDataInDatabase.username = username;
        userDataInDatabase.tel = tel;

        PlayerPrefs.SetString("deviceID", userDataInDatabase.deviceID);
        PlayerPrefs.SetString("username", userDataInDatabase.username);
        PlayerPrefs.SetString("tel", userDataInDatabase.tel);
       
    }
    public void SaveUiD(string uid)
    {
        userDataInDatabase.uid = uid;
        PlayerPrefs.SetString("uid", userDataInDatabase.uid);

    }
    public void SaveLastChapterPlayed()
    {
        lastChapterPlayedKey = Data.Instance.capitulosData.activeCapitulo.key;
        PlayerPrefs.SetString("lastChapterPlayedKey", lastChapterPlayedKey);
    }
    public void SetAnswer(int id, float timer)
    {
        AnswersData ad = new AnswersData();
        ad.respuesta = id;
        ad.timer = timer;
        answers.Add(ad);
    }
    public void Reset()
    {
        answers.Clear();
    }
}
