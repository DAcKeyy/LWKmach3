using UnityEngine.Networking;
using UnityEngine;
using System;

public enum ServerErrors
{
    Unknown,
    EndedCoupons
}

public class MachThreeServerMeassage : MonoBehaviour
{

    WebSender Sender = new WebSender();
    public static Action<ServerErrors> ServerError;
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
        Chest chest = new Chest();
        chest.json = response;
        chest.isOpend = false;

        GlobalDataBase.LootChests.ChestsList.Add(chest);
        foreach (var i in GlobalDataBase.LootChests.ChestsList)
            Debug.Log(i.json);
    }

    void Errors(string response)
    {
        Debug.Log("Error: " + response);
        ServerError(ResolveError(response));
    }

    ServerErrors ResolveError(string json)
    {
        System.Text.RegularExpressions.Regex endedCoupons = new System.Text.RegularExpressions.Regex(@"Coupon not found");

        if(endedCoupons.IsMatch(json))
        {
            return ServerErrors.EndedCoupons;
        }

        return ServerErrors.Unknown;
    }
}
