using UnityEngine.Networking;
using UnityEngine;
using System;

public class MachThreeServerMeassage : MonoBehaviour
{
    private int GetChestIncremention;

    WebSender Sender = new WebSender();

    private void Start()
    {
        TaskManager.TaskComplete += GetChest;

        GlobalDataBase.MachThreeSessions++;

        string Json = "{ \"data\": {\"type\": \"games\",\"attributes\": {\"game_id\":\"" + GlobalDataBase.MachThreeSessions
            + "\",\"user_id\": \"" + GlobalDataBase.UserID
            + "\",\"session_date\": \"" + DateTime.Now 
            + "\"}}}";

        Debug.Log(Json);
    }

    void GetChest()
    {
        GetChestIncremention++;

        Debug.Log("GetChestIncremention: " + GetChestIncremention);
        if(GetChestIncremention == 3)
        {
            var webRequest = UnityWebRequest.Get(URLStruct.LootBox);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            webRequest.SetRequestHeader("Content-Type", "application/vnd.api+json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);
            StartCoroutine(Sender.SendWebRequest(webRequest, ChestResponse, Errors));

            GetChestIncremention = 0;
        }
    }

    void ChestResponse(string response)
    {
        Debug.Log("Response: " + response);
        GlobalDataBase.ChestJSONs.Add(response);
    }

    void Errors(string response)
    {
        Debug.Log("Error: " + response);
    }
}
