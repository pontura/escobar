using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using FullSerializer;

public class UsersListView : MainScreen
{
    public ResultViewLine line;
    public Transform container;

    public override void OnEnabled()
    {
        Utils.RemoveAllChildsIn(container);
        LoadUsers();
    }
    void LoadUsers()
    {
        string url = Data.Instance.firebaseAuthManager.databaseURL + "/usuarios.json?auth=" + Data.Instance.userData.token;
        Debug.Log("LoadUsers " + url);
        RestClient.Get(url).Then(response =>
        {
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(response.Text);
            Dictionary<string, UsersData.DataBasic> results = null;
            serializer.TryDeserialize(data, ref results);
            foreach (UsersData.DataBasic d in results.Values)
            {
                Data.Instance.usersData.CheckToAdd(d);
            }
            Ready();
        });
    }
    void Ready()
    {
        Utils.RemoveAllChildsIn(container);
        foreach (UsersData.DataBasic data in Data.Instance.usersData.all)
        {
            ResultViewLine newLine = Instantiate(line);
            newLine.transform.SetParent(container);
            newLine.InitOnlyUserData(data);
            newLine.transform.transform.localScale = Vector3.one;
        }
    }
}
