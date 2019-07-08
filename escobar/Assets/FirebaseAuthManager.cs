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
      //  Events.OnFirebaseLogin += OnFirebaseLogin;
        Events.OnSaveUserToServer += OnSaveUserToServer;
        Events.OnGetServerData += OnGetServerData;
        //Events.OnFirebaseLogin += OnFirebaseLogin;
    }
    //void OnFirebaseLogin()
    //{
    //    //print("OnFirebaseLogin _______anoninmo");
    //    //SignUpUserAnon();
    //}
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
    public void SignUpUserByEmail(string email, string password)
    {

        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
               // user.localId = response.localId;
               // GetUsername();
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
    public void Send()
    {
        Debug.Log("__________Save Results");

        TriviaData data = new TriviaData();
        UserData userData = Data.Instance.userData;
        data.uid = Data.Instance.userData.userDataInDatabase.uid;
        data.respuestas = userData.answers;
        int score = 0;
        foreach (UserData.AnswersData d in data.respuestas)
        {
            score += 1000 - (int)(d.timer * 100);
            if (d.respuesta == 0)
                score += 10000;
        }
        data.score = score;

        string json = JsonUtility.ToJson(data);
        string capituloKey = Data.Instance.capitulosData.activeCapitulo.key;
        // reference.Child("capitulos_participantes").Child(Data.Instance.capitulosData.activeCapitulo.key).Child("participantes").Child(Data.Instance.userData.userDataInDatabase.uid).SetRawJsonValueAsync(json);

        string url = "https://triviaescobar.firebaseio.com/capitulos_participantes/" + Data.Instance.capitulosData.activeCapitulo.key + "/" + Data.Instance.userData.userDataInDatabase.uid + "/.json?auth=" + idToken;
        RestClient.Post(url, data);

        Data.Instance.userData.SaveLastChapterPlayed();
    }
    public void SaveCapitulo(CapitulosData.Capitulo capitulo, string capituloKey = "")
    {
        //Push:
        if (capituloKey == "")
        {
            string url = "https://triviaescobar.firebaseio.com/capitulos.json?auth=" + idToken;
            RestClient.Post(url, capitulo);
        }
        //Update:
        else
        {
            string url = "https://triviaescobar.firebaseio.com/capitulos/" + capituloKey + "/.json?auth=" + idToken;
            RestClient.Put(url, capitulo);
            print("Update Capitulo url : " + url);
        }
    }
    public void SaveTraining(TrainingData.Question question, string key = "")
    {
        //Push:
        if (key == "")
        {
            string url = "https://triviaescobar.firebaseio.com/entrenamiento.json?auth=" + idToken;
            RestClient.Post(url, question);
            print("SaveTraining url : " + url);
        }
        //Update:
        else
        {
            string url = "https://triviaescobar.firebaseio.com/entrenamiento/" + key + "/.json?auth=" + idToken;
            RestClient.Put(url, question);
            print("Update Training url : " + url);
        }
    }
    public void DeleteTraining(string trainingKey = "")
    {
        Debug.Log("_______DEBERIA BORRAR:");
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