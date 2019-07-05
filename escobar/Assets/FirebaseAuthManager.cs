using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using System;

public class FirebaseAuthManager : MonoBehaviour
{

    [Serializable]
    public class SignResponse
    {
        public string localId;
        public string idToken;
    }

    public string databaseURL = "https://triviaescobar.firebaseio.com";
    public string AuthKey = "AIzaSyCMDm_PBkjWP-evqXw4saW-Ow_54_exUCA";

    public string idToken;

    private string getLocalId;
    fsSerializer serializer;

    public bool isDone;

    void Awake()
    {
        Events.OnFirebaseLogin += OnFirebaseLogin;
        Events.OnSaveUserToServer += OnSaveUserToServer;
        Events.OnGetServerData += OnGetServerData;
        //Events.OnFirebaseLogin += OnFirebaseLogin;
    }
    void OnFirebaseLogin()
    {
        print("OnFirebaseLogin _______anoninmo");
        SignUpUserAnon();
    }
    public void SignUpUserAnon()
    {
        string userData = "{\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(
            response =>
            {
                
                idToken = response.idToken;
                if (Data.Instance.userData.userDataInDatabase.uid.Length == 0)
                {
                    Data.Instance.userData.SaveUiD(response.localId);
                    OnSaveUserToServer();
                }
                isDone = true;
            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }
    void OnSaveUserToServer()
    {
        UserData.UserDataInDatabase userData = Data.Instance.userData.userDataInDatabase;
        string url = "https://triviaescobar.firebaseio.com/usuarios/" + userData.uid + "/.json?auth=" + idToken;
        RestClient.Put(url, userData);
        print("OnSaveUserDate url : " + url);
    }
    public void OnGetServerData(string childName, System.Action<object> OnReady, string orderby = "", int limitToLast = 1000)
    {
        RestClient.Get<CapitulosData.Capitulo>(databaseURL + "/" + childName + ".json?auth=" + idToken).Then(response =>
        {
            return response;
        });
    }
    public void SaveNewTraining(TrainingData.Question question)
    {
        string url = "https://triviaescobar.firebaseio.com/entrenamiento.json?auth=" + idToken;
        RestClient.Post(url, question);
        print("OnSaveUserDate url : " + url);
    }











    public void OnSubmit()
    {
        serializer = new fsSerializer();
        PostToDatabase();
    }

    private void PostToDatabase(bool emptyScore = false)
    {
        if (emptyScore)
        {
           // user.score = 0;
        }
       // RestClient.Put(databaseURL + "/" + user.localId + ".json?auth=" + idToken, user);
    }

    private void RetrieveFromDatabase()
    {
        //RestClient.Get<User>(databaseURL + "/" + getLocalId + ".json?auth=" + idToken).Then(response =>
        //{
        //    user = response;
        //    //UpdateScore();
        //});
    }
    //public void SignUpUserAnon()
    //{
    //    string userData = "{\"returnSecureToken\":true}";
    //    RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(
    //        response =>
    //        {
    //           // user.username = "nada";
    //            idToken = response.idToken;
    //         //   user.localId = response.localId;
    //            PostToDatabase(true);
    //            print("anoninmo");
    //            GetServerTime();
    //        }).Catch(error =>
    //        {
    //            Debug.Log(error);
    //        });
    //}
    public void SignUpUserByEmail(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(
            response =>
            {
              //  user.username = username;
                idToken = response.idToken;
               // user.localId = response.localId;
                PostToDatabase(true);
                //GetServerTime();
            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }

    public void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
              //  user.localId = response.localId;
                GetUsername();
            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }

    private void GetUsername()
    {
        //RestClient.Get<User>(databaseURL + "/" + user.localId + ".json?auth=" + idToken).Then(response =>
        //{
        //    // playerName = response.userName;
        //});
    }

    private void GetLocalId()
    {
        RestClient.Get(databaseURL + ".json?auth=" + idToken).Then(response =>
        {
         //   string username = user.username;

            fsData userData = fsJsonParser.Parse(response.Text);
            //Dictionary<string, User> users = null;
            //serializer.TryDeserialize(userData, ref users);

            //foreach (User user in users.Values)
            //{
            //    if (user.username == username)
            //    {
            //        getLocalId = user.localId;
            //        RetrieveFromDatabase();
            //        break;
            //    }
            //}


        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
    public void GetServerTime()
    {
        RestClient.Get("https://triviaescobar.firebaseio.com/.info/serverTimeOffset.json?auth=" + idToken).Then(response =>
        {
            print("servertime: " + response);
        }).Catch(error =>
        {
            Debug.Log(error);
        });
    }
    void PostScore()
    {
        //User user = new User();
        //user.uid = "qQ4WfifTk2eHdanVa5oR31pwTiJ3";
        //user.score = 123;
       // string url = "https://triviaescobar.firebaseio.com/participantes/" + user.uid + "/.json?auth=" + idToken;
       // print(url);
      //  RestClient.Put(url, user);
    }
    void GetScore()
    {
        //RestClient.Get<User>("https://triviaescobar.firebaseio.com/participantes/" + user.uid + ".json").Then(response =>
        //{
        //    user = response;
        //}
      //  );
    }
}