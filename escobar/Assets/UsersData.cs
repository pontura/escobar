using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UsersData : MonoBehaviour
{
    public List<DataBasic> all;

    [Serializable]
    public class DataBasic
    {
        public string uid;
        public string tel;
        public string username;
        public string edad;
    }
    void Start()
    {
        Events.OnUserBasicData += OnUserBasicData;
    }
    public DataBasic GetUserData(string uid)
    {
        foreach (DataBasic data in all)
        {
            if (data.uid == uid)
                return data;
        }
        return null;
    }
    
    void OnUserBasicData(DataBasic data)
    {
        print("::::::::::::::::::::::: new user " + data);
        all.Add(data);
    }
    public void CheckToAdd(DataBasic data)
    {
        foreach(DataBasic db in all)
        {
            if (db.uid == data.uid)
                return;
        }
        all.Add(data);
    }
    
}
