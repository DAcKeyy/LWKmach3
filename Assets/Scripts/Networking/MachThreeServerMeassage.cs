using UnityEngine.Networking;
using UnityEngine;
using System;

public class MachThreeServerMeassage : MonoBehaviour
{
    WebSender Sender = new WebSender();

    private void Start()
    {
        ChestAnimationController.ChestCompleted += GetChest;

    }

    private void OnDisable()
    {
        ChestAnimationController.ChestCompleted -= GetChest;
    }

    void GetChest()
    {
        var webRequest = UnityWebRequest.Get(URLStruct.LootBox);
        StartCoroutine(Sender.SendWebRequest(webRequest, ChestResponse, Errors));
    }

    void ChestResponse(string response)
    {
        GlobalDataBase.LootChests.Chest chest = new GlobalDataBase.LootChests.Chest();
        chest.json = response;
        chest.isOpend = false;

        GlobalDataBase.LootChests.ChestsList.Add(chest);
        foreach (var i in GlobalDataBase.LootChests.ChestsList)
            Debug.Log(i.json);
    }

    void Errors(string response)
    {
        Debug.Log("Error: " + response);
    }
}
