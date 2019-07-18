using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MainScreen
{
    public InputField usernameField;
    public InputField edadField;
    public InputField telField;
    public Text debbugField;
    bool isNew;

    public override void OnEnabled()
    {
        Debug.Log("RegisterScreen");

       // base.OnInit();
        debbugField.text = "";

        if (Data.Instance.userData.userDataInDatabase.username == "")
            isNew = true;

        if (!isNew)
            usernameField.text = Data.Instance.userData.userDataInDatabase.username;
        if (Data.Instance.userData.userDataInDatabase.tel.Length > 1)
            telField.text = Data.Instance.userData.userDataInDatabase.tel;
        if (Data.Instance.userData.userDataInDatabase.edad.Length > 0)
            edadField.text = Data.Instance.userData.userDataInDatabase.edad;
    }
    public void Register()
    {
        CancelInvoke();
        if (usernameField.text.Length < 2)
            debbugField.text = "Agrega un nombre real";
        if (edadField.text == "")
            edadField.text = "Agrega tu edad";
        else if (telField.text.Length < 8)
            debbugField.text = "Agrega un teléfono real";
        else
        {
            debbugField.text = "Enviando datos...";
            Data.Instance.userData.SaveUser(usernameField.text, telField.text, edadField.text);
            

            if (isNew)
            {
                LoopTillUserRegistered();
            }
            else
            {
                Data.Instance.firebaseAuthManager.OnSaveUserToServer();
                UI.Instance.screensManager.LoadScreen(1, false);
            }
        }
        Invoke("Reset", 2);
    }
    void Reset()
    {
        debbugField.text = "";
    }
    void LoopTillUserRegistered()
    {
        if (Data.Instance.userData.userDataInDatabase.uid.Length < 1)
        {
            Invoke("LoopTillUserRegistered", 0.5f);
        }
        else
        {
           // Data.Instance.serverManager.SaveUserData();
            UI.Instance.screensManager.LoadScreen(0, true);
            Data.Instance.firebaseAuthManager.OnSaveUserToServer();
        }
    }
}
