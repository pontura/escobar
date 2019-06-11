using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
	public MainScreen[] all;

    public MainScreen activeScreen;
	MainScreen lastActiveScreen;
    public bool isAdmin;

    bool loading;

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
    }
    void LoopTillFirebaseDone()
    {
        if (Data.Instance.serverManager.isDone)
            OnFirebaseDone();
        else
            Invoke("LoopTillFirebaseDone", 0.2f);
    }
    void OnFirebaseDone()
    {
        Debug.Log("OnFirebaseDone");
       
        if(isAdmin)
        {
            Data.Instance.serverManager.SignInWithEmailAndPassword("yaguar@gmail.com", "yaguar");
            LoadScreen(0, true);
            return;
        }
        bool logged = Data.Instance.userData.IsLogged();

        if (!logged)
        {
            Debug.Log("Abre Register");
            LoadScreen(3, true);
        }
        else
        {
            Debug.Log("App Registed");
            LoadScreen(0, true);
        }
    }
	public void LoadScreen(int id, bool isRight)
	{
        Debug.Log("LoadScreen " + id + " loading: " + loading + " activeScreen: " + activeScreen);

        if (loading)
			return;

        Events.OnUIFX("swipe");

		loading = true;
		if (activeScreen != null) {
			activeScreen.SetCenterPosition ();
			activeScreen.MoveTo (isRight);
			lastActiveScreen = activeScreen;
		}

        activeScreen = all [id];
        activeScreen.gameObject.SetActive (true);
        activeScreen.SetInitialPosition (isRight);
        activeScreen.MoveTo (isRight);

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
        Debug.Log("ResetAll");
        foreach (MainScreen mainScreen in all) {
			mainScreen.gameObject.SetActive (false);
		}
	}

}
