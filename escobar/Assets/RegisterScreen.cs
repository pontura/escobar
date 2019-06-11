using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MainScreen
{
    public InputField usernameField;
    public InputField telField;
    public Text debbugField;
    bool isNew;

    public override void OnEnabled()
    {
        Debug.Log("RegisterScreen");

       // base.OnInit();
        debbugField.text = "";

        if (Data.Instance.userData.username == "")
            isNew = true;

        if (!isNew)
            usernameField.text = Data.Instance.userData.username;
        if (Data.Instance.userData.tel.Length > 1)
            telField.text = Data.Instance.userData.tel;
    }
    public void Register()
    {
        CancelInvoke();
        if (usernameField.text.Length < 2)
            debbugField.text = "Agrega un nombre real";
        else if (telField.text.Length < 8)
            debbugField.text = "Agrega un teléfono real";
        else
        {
            debbugField.text = "Enviando datos...";
            Data.Instance.userData.SaveUser(usernameField.text, telField.text);
            
            if (isNew)
            {
                Events.OnFirebaseLogin();
                LoopTillUserRegistered();
            }
            else
                UI.Instance.screensManager.LoadScreen(1, false);
        }
        Invoke("Reset", 2);
    }
    void Reset()
    {
        debbugField.text = "";
    }
    void LoopTillUserRegistered()
    {
        if (Data.Instance.userData.uid.Length < 1)
        {
            Invoke("LoopTillUserRegistered", 0.5f);
        }
        else
        {
            UI.Instance.screensManager.LoadScreen(0, true);
        }
    }
}
