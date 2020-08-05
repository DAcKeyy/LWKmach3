using LWT.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShuter : MonoBehaviour
{
    //[SerializeField] LevelLoader loader;

    private void OnEnable()
    {
        MachThreeServerMeassage.ServerError += ShutDown;
    }
    private void OnDisable()
    {
        MachThreeServerMeassage.ServerError -= ShutDown;
    }

    private void ShutDown(ServerErrors error)
    {
        switch(error)
        {
            case ServerErrors.EndedCoupons:
                GlobalDataBase.Error = "The game has closed due to the lack of coupons in the system";
                break;
            case ServerErrors.Unknown:
                GlobalDataBase.Error = "The game was closed due to an unknown cause";
                break;
        }
        
        FindObjectOfType<LevelLoader>().LoadScene("Start");
    }
}
