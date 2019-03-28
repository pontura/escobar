using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public string username;
    public string tel;
    public string deviceID;
    public string lastTriviaPlayed;
    public List<int> answers;

    void Start()
    {
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
    }
    public void SetAnswer(int id)
    {
        answers.Add(id);
    }
}
