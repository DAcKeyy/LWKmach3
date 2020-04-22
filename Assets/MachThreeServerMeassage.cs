using UnityEngine;
using System;

public class MachThreeServerMeassage : MonoBehaviour
{


    private void Start()
    {
        GlobalDataBase.MachThreeSessions++;

        string Json = "{ \"data\": {\"type\": \"games\",\"attributes\": {\"game_id\":\"" + GlobalDataBase.MachThreeSessions
            + "\",\"user_id\": \"" + GlobalDataBase.UserID
            + "\",\"session_date\": \"" + DateTime.Now 
            + "\"}}}";

        Debug.Log(Json);
    }
}
