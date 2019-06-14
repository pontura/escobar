using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserData : MonoBehaviour
{
    public string lastChapterPlayedKey;
    public string uid;
    public string username;
    public string tel;
    public string deviceID;
    public List<AnswersData> answers;

    [Serializable]
    public class AnswersData
    {
        public int respuesta;
        public float timer;
    }

    void Awake()
    {
        lastChapterPlayedKey = PlayerPrefs.GetString("lastChapterPlayedKey", "");
        username = PlayerPrefs.GetString("username", "");
        tel = PlayerPrefs.GetString("tel", "");
        deviceID = PlayerPrefs.GetString("deviceID", "");
    }
    public bool IsLogged()
    {
        if (deviceID.Length<2)
            return false;
        return true;
    }
    public void SaveUser(string username, string tel)
    {
        this.deviceID = SystemInfo.deviceUniqueIdentifier;
        this.username = username;        
        this.tel = tel;

        PlayerPrefs.SetString("deviceID", deviceID);
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("tel", tel);

        Data.Instance.serverManager.SaveUserData();
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
