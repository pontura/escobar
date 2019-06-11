using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

public class ServerManager : MonoBehaviour
{
    int diferenciaHoraria = 243;
    DatabaseReference reference;
    Firebase.FirebaseApp app;
    public bool isDone;

    void Awake()
    {
        Events.OnGetServerData += OnGetServerData;
        Events.OnFirebaseLogin += OnFirebaseLogin;
    }
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://triviaescobar.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("App ready. OnFirebaseDone");
                isDone = true;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }



    //Admin:
    public void SignInWithEmailAndPassword(string email, string password)
    {
        Debug.Log("OnFirebaseLogin");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPassword was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPassword encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Data.Instance.userData.uid = newUser.UserId;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }


    //All USers:
    void OnFirebaseLogin()
    {
        Debug.Log("OnFirebaseLogin");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

       auth.SignInAnonymouslyAsync().ContinueWith(task => {
           if (task.IsCanceled)
           {
               Debug.LogError("SignInAnonymouslyAsync was canceled.");
               return;
           }
           if (task.IsFaulted)
           {
               Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
               return;
           }
           Firebase.Auth.FirebaseUser newUser = task.Result;
           Data.Instance.userData.uid = newUser.UserId;
           Debug.LogFormat("User signed in successfully: {0} ({1})",  newUser.DisplayName, newUser.UserId);
       });
    }


    void OnDestroy()
    {
        Events.OnGetServerData -= OnGetServerData;
    }    

    public void Send()
    {
        TriviaData data = new TriviaData();
        UserData userData = Data.Instance.userData;
        data.uid = Data.Instance.userData.uid;
        data.respuestas = userData.answers;

        string json = JsonUtility.ToJson(data);
        string capituloKey = Data.Instance.capitulosData.activeCapitulo.key;
        reference.Child("capitulos").Child(Data.Instance.capitulosData.activeCapitulo.key).Child("participantes").Child(Data.Instance.userData.uid).SetRawJsonValueAsync(json);
        Data.Instance.userData.SaveLastChapterPlayed();
    }
    public void SaveUserData()
    {
        FirebaseUserData fUserData = new FirebaseUserData();
        fUserData.uid = Data.Instance.userData.uid;
        fUserData.username = Data.Instance.userData.username;
        fUserData.tel = Data.Instance.userData.tel;
        fUserData.deviceID = Data.Instance.userData.deviceID;

        string json = JsonUtility.ToJson(fUserData);
        reference.Child("usuarios").Child(Data.Instance.userData.uid).SetRawJsonValueAsync(json);
    }


    public void OnGetServerData(string childName, System.Action<DataSnapshot> OnReady)
    {
        Debug.Log("OnGetServerData " + childName);

        FirebaseDatabase.DefaultInstance
       .GetReference(childName)
       .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
               Debug.Log("error " + task);
           }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
               OnReady(snapshot);
           }
        }
        );
    }
    public void UpdateData(string table, string key, object obj)
    {
        string json = JsonUtility.ToJson(obj);
        reference.Child(table).Child(key).SetRawJsonValueAsync(json);
        Debug.Log("UpdateQuestion " + json);    
    }
    public void PushData(string table, object obj)
    {
        string json = JsonUtility.ToJson(obj);
        reference.Child(table).Push().SetRawJsonValueAsync(json);
        Debug.Log("PushQuestion " + json);
    }
    public void DeleteQuestion(string key)
    {
        reference.Child("entrenamiento").Child(key).RemoveValueAsync();
        Debug.Log("DeleteQuestion " + key);
    }


}
