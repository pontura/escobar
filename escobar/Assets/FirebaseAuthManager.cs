using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using UnityEngine.Networking;

public class FirebaseAuthManager : MonoBehaviour
{

    [Serializable]
    public class SignResponse
    {
        public string localId;
        public string idToken;
        public string refreshToken;
    }
    [Serializable]
    public class RefreshData
    {
        public string refresh_token;
        public string id_token;
    }

    public string databaseURL;
    public string AuthKey;

    fsSerializer serializer;

    public bool isDone;

    void Awake()
    {
        Events.OnGetServerData += OnGetServerData;
    }

    //    curl 'https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyCustomToken?key=[API_KEY]' \
    // 'Content-Type: application/json' \
    // '{"token":"[CUSTOM_TOKEN]","returnSecureToken":true}'
    public void VerifyToken()
    {
        StartCoroutine(Upload());
    }
    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "refresh_token");
        form.AddField("refresh_token", Data.Instance.userData.refreshToken);

        form.headers["Content-Type"] = "application/x-www-form-urlencoded";
        UnityWebRequest www = UnityWebRequest.Post("https://securetoken.googleapis.com/v1/token?key=" + AuthKey, form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            RefreshData response = JsonUtility.FromJson< RefreshData>( www.downloadHandler.text );

            Debug.Log("new id_token    " + response.id_token);
            Debug.Log("old id_token    " + Data.Instance.userData.token);

            Debug.Log("new refresh_token       " + response.refresh_token);
            Debug.Log("old freshTokend_token   " + Data.Instance.userData.refreshToken);

            Data.Instance.userData.SaveToken(response.id_token);
            Data.Instance.userData.SaveRefreshToken(response.refresh_token);

            isDone = true;
        }
    }


    public void SignUpUserAnon()
    {
        string userData = "{\"uid\":\"" + PlayerPrefs.GetString("uid", "") + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(
            response =>
            {
                Data.Instance.userData.SaveToken(response.idToken);
                Data.Instance.userData.SaveRefreshToken(response.refreshToken);
                print("LOGUEADO localId: " + response.localId);
                if (Data.Instance.userData.userDataInDatabase.uid.Length == 0)
                {
                    Data.Instance.userData.SaveUiD(response.localId);
                    OnSaveUserToServer();
                }
                isDone = true;
                GetServerTime();
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
                Data.Instance.userData.SaveToken(response.idToken);
                if (Data.Instance.userData.userDataInDatabase.uid.Length == 0)
                {
                    Data.Instance.userData.SaveUiD(response.localId);
                    //OnSaveUserToServer();
                }
                isDone = true;
            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }


    public void OnSaveUserToServer()
    {
        UserData.UserDataInDatabase userData = Data.Instance.userData.userDataInDatabase;
        string url = databaseURL + "/usuarios/" + userData.uid + "/.json?auth=" + Data.Instance.userData.token;
        RestClient.Put(url, userData);
        print("OnSaveUserDate url : " + url);
    }
    public void OnGetServerData(string childName, System.Action<object> OnReady, string orderby = "", int limitToLast = 1000)
    {
        RestClient.Get<CapitulosData.Capitulo>(databaseURL + "/" + childName + ".json?auth=" + Data.Instance.userData.token).Then(response =>
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

        string url = databaseURL + "/capitulos_participantes/" + Data.Instance.capitulosData.activeCapitulo.key + "/participantes/" + Data.Instance.userData.userDataInDatabase.uid + "/.json?auth=" + Data.Instance.userData.token;
        RestClient.Put(url, data);

        Data.Instance.userData.SaveLastChapterPlayed();
    }
    public void SaveCapitulo(CapitulosData.Capitulo capitulo, string capituloKey = "")
    {
        //Push:
        if (capituloKey == "")
        {
            string url = databaseURL + "/capitulos.json?auth=" + Data.Instance.userData.token;
            RestClient.Post(url, capitulo);
        }
        //Update:
        else
        {
            string url = databaseURL + "/capitulos/" + capituloKey + "/.json?auth=" + Data.Instance.userData.token;
            RestClient.Put(url, capitulo);
            print("Update Capitulo url : " + url);
        }
    }
    public void SaveTraining(TrainingData.Question question, string key = "")
    {
        //Push:
        if (key == "")
        {
            string url = databaseURL + "/entrenamiento.json?auth=" + Data.Instance.userData.token;
            RestClient.Post(url, question);
            print("SaveTraining url : " + url);
        }
        //Update:
        else
        {
            string url = databaseURL + "/entrenamiento/" + key + "/.json?auth=" + Data.Instance.userData.token;
            RestClient.Put(url, question);
            print("Update Training url : " + url);
        }
    }
    public void DeleteTraining(string trainingKey = "")
    {
        Debug.Log("_______DEBERIA BORRAR DeleteTraining:");
        string url = databaseURL + "/entrenamiento/" + trainingKey + "/.json?auth=" + Data.Instance.userData.token;
        RestClient.Delete(url);
        print("Update Training url : " + url);
    }
    public void DeleteCapitulo(string capituloKey = "")
    {       
        string url = databaseURL + "/capitulos/" + capituloKey + "/.json?auth=" + Data.Instance.userData.token;
        Debug.Log("_______DEBERIA BORRAR DeleteCapitulo: " + url);
        RestClient.Delete(url);
        print("Update Training url : " + url);
    }
    public void GetServerTime()
    {
        RestClient.Get(databaseURL + "/.info/serverTimeOffset.json?auth=" + Data.Instance.userData.token).Then(response =>
        {
            print("________servertime: " + response);
        }).Catch(error =>
        {
            Debug.Log(error);
        });
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
              //  idToken = response.idToken;
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
        //RestClient.Get(databaseURL + ".json?auth=" + idToken).Then(response =>
        //{
        // //   string username = user.username;

        //    fsData userData = fsJsonParser.Parse(response.Text);
        //    //Dictionary<string, User> users = null;
        //    //serializer.TryDeserialize(userData, ref users);

        //    //foreach (User user in users.Values)
        //    //{
        //    //    if (user.username == username)
        //    //    {
        //    //        getLocalId = user.localId;
        //    //        RetrieveFromDatabase();
        //    //        break;
        //    //    }
        //    //}


        //}).Catch(error =>
        //{
        //    Debug.Log(error);
        //});
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