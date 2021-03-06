﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
	public MainScreen[] all;

    public MainScreen activeScreen;
	MainScreen lastActiveScreen;
    public bool isAdmin;
    public float timeToTransition = 1;
    public bool loading;
    int id;

    void Start()
    {
        int id = 0;
        foreach (MainScreen mainScreen in all)
        {
            mainScreen.Init(this, id);
            id++;
        }
        ResetAll();
        LoopTillFirebaseDone();
        if (isAdmin)
        {
            Data.Instance.firebaseAuthManager.SignUpUserByEmail("yaguar@gmail.com", "yaguar");
        }
        else
        {
            
            if (Data.Instance.userData.token != "")
            {
                LoadScreen(0, true);
                Data.Instance.firebaseAuthManager.VerifyToken();
            }
            else
            {
                // Data.Instance.firebaseAuthManager.SignUpUserAnon();
                Debug.Log("Abre Register");
                LoadScreen(3, true);
            }
        }
       
    }
    void LoopTillFirebaseDone()
    {
        if (Data.Instance.firebaseAuthManager.isDone)
            OnFirebaseDone();
        else
            Invoke("LoopTillFirebaseDone", 0.2f);
    }
    void OnFirebaseDone()
    {       
        if(isAdmin)
        {
            //Data.Instance.firebaseAuthManager.SignUpUserByEmail("yaguar@gmail.com", "yaguar");
            LoadScreen(0, true);
            return;
        }
        bool logged = Data.Instance.userData.IsLogged();
    }
	public void LoadScreen(int id, bool isRight)
	{
        this.id = id;
        Debug.Log("LoadScreen " + id + " loading: " + loading + " activeScreen: " + activeScreen);

        if (loading)
			return;

        Events.OnUIFX("swipe");

		loading = true;
		if (activeScreen != null) {
			activeScreen.SetCenterPosition ();
			activeScreen.MoveTo (isRight, timeToTransition);
			lastActiveScreen = activeScreen;
		}

        activeScreen = all [id];
        activeScreen.gameObject.SetActive (true);
        activeScreen.SetInitialPosition (isRight);
        activeScreen.MoveTo (isRight, timeToTransition);

    }
	public void OnTransitionDone()
	{
        if (!loading)
			return;
		loading = false;
		if (lastActiveScreen != null) {
			lastActiveScreen.gameObject.SetActive (false);
			lastActiveScreen.OnReset ();
		}
		activeScreen.OnInit ();
	}
	public void ResetAll()
	{
        foreach (MainScreen mainScreen in all) {
			mainScreen.gameObject.SetActive (false);
		}
	}
    void OnApplicationFocus(bool pauseStatus)
    {
        if (Time.time < 2 || !Data.Instance.firebaseAuthManager.isDone)
            return;
        if(id==2 && pauseStatus)
        {
            Events.OnResetApp();
            LoadScreen(0, true);
        }
    }

}
