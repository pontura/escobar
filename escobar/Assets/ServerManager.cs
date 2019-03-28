using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    DatabaseReference reference;
    Firebase.FirebaseApp app;

    void Start()
    {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://triviaescobar.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            print("1 " + dependencyStatus);
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                
                // Create and hold a reference to your FirebaseApp, i.e.
               //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
                print("App ready");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["capitulo"] = "Capitulo 1";
        result["id"] = "ASDASD";
        result["nombre"] = "Pontura";
        result["telefono"] = "123423423423";
        result["email"] = "pontura@gmail.com";
        int[] respuestas = new int[2];
        respuestas[0] = 1;
        respuestas[1] = 1;
        result["respuestas"] = respuestas;
        return result;
    }

    public void Send()
    {
        print("Send");
        TriviaData data = new TriviaData();
        data.capitulo = 1;
        UserData userData = Data.Instance.userData;
        data.id = userData.deviceID;
        data.nombre = userData.username;
        data.telefono = userData.tel;
        //data.email = "pontura@gmail.com";
        //int[] respuestas = new int[2];
        //respuestas[0] = 1;
        //respuestas[1] = 1;
        data.respuestas = userData.answers;

        string json = JsonUtility.ToJson(data);
        reference.Child("usuarios").Push().SetRawJsonValueAsync(json);
        print("Sended " + json);
    }
    void Read()
    {
      
    }
}
