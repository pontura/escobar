using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
	public MainScreen[] all;

	MainScreen activeScreen;
	MainScreen lastActiveScreen;

	bool loading;

	void Start()
	{
		int id = 0;
		foreach (MainScreen mainScreen in all) {
			mainScreen.Init (this, id);
			id++;
		}
		ResetAll ();
		LoadScreen (0, true);
	}
	public void LoadScreen(int id, bool isRight)
	{
        print("LoadScreen " + id + loading);

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
		foreach (MainScreen mainScreen in all) {
			mainScreen.gameObject.SetActive (false);
		}
	}

}
