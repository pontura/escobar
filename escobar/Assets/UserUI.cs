﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUI : MonoBehaviour
{
    public Text username;
    public Text tel;

    void OnEnable()
    {
        if (Data.Instance != null)
        {
            username.text = Data.Instance.userData.userDataInDatabase.username;
            tel.text = Data.Instance.userData.userDataInDatabase.tel;
        }
    }
    public void Edit()
    {
        UI.Instance.screensManager.LoadScreen(3, true);
    }
}
