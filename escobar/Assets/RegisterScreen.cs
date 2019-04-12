using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MainScreen
{
    public InputField usernameField;
    public InputField telField;
    public Text debbugField;

    public override void OnInit()
    {
        base.OnInit();
        debbugField.text = "";

        if (Data.Instance.userData.username.Length > 1)
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
            UI.Instance.screensManager.LoadScreen(1, false);
        }
        Invoke("Reset", 2);
    }
    void Reset()
    {
        debbugField.text = "";
    }
}
