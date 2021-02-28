using LWT.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShuter : MonoBehaviour
{
    [SerializeField] Counter Chest;
    [SerializeField] Counter Time;

    private void OnEnable()
    {
        MachThreeServerMeassage.ServerError += ShutDown;
        UpTimer.GameIsOver += SetStatistics;
    }
    private void OnDisable()
    {
        MachThreeServerMeassage.ServerError -= ShutDown;
        UpTimer.GameIsOver -= SetStatistics;
    }

    private void ShutDown(ServerErrors error)
    {
        switch (error)
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

    private void SetStatistics()
    {
        Chest.SetText(GlobalDataBase.ChestEarned);
        Time.SetText($"{Convert.ToInt32(GlobalDataBase.RoundTime) / 60} : {Convert.ToInt32(GlobalDataBase.RoundTime) % 60}");
    }
}
