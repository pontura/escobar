﻿using System.Collections;
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
    public string token;
    public string refreshToken;
    [Serializable]
    public class UserDataInDatabase
    {
        public string username;
        public string tel;
        public string uid;
        public string edad;
        public string deviceID;
        public string apellido;
        public string email;
        public string dni;
        
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
        userDataInDatabase.edad = PlayerPrefs.GetString("edad", "");

        userDataInDatabase.apellido = PlayerPrefs.GetString("apellido", "");
        userDataInDatabase.email = PlayerPrefs.GetString("email", "");
        userDataInDatabase.dni = PlayerPrefs.GetString("dni", "");

        token = PlayerPrefs.GetString("token", "");
        refreshToken = PlayerPrefs.GetString("refreshToken", "");
    }
    public bool IsLogged()
    {
        if (userDataInDatabase.deviceID.Length<2)
            return false;
        return true;
    }
    public void SaveUser(string username, string tel, string edad, string email, string apellido, string dni)
    {
        userDataInDatabase.deviceID = SystemInfo.deviceUniqueIdentifier;
        userDataInDatabase.username = username;
        userDataInDatabase.tel = tel;
        userDataInDatabase.edad = edad;
        userDataInDatabase.email = email;
        userDataInDatabase.apellido = apellido;
        userDataInDatabase.dni = dni;

        PlayerPrefs.SetString("deviceID", userDataInDatabase.deviceID);
        PlayerPrefs.SetString("username", userDataInDatabase.username);
        PlayerPrefs.SetString("edad", userDataInDatabase.edad);
        PlayerPrefs.SetString("tel", userDataInDatabase.tel);

        PlayerPrefs.SetString("email", userDataInDatabase.email);
        PlayerPrefs.SetString("apellido", userDataInDatabase.apellido);
        PlayerPrefs.SetString("dni", userDataInDatabase.dni);

    }
    public void SaveToken(string token)
    {
        PlayerPrefs.SetString("token", token);
        this.token = token;
    }
    public void SaveRefreshToken(string token)
    {
        PlayerPrefs.SetString("refreshToken", token);
        this.refreshToken = token;
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
