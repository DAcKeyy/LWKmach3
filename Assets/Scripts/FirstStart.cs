using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Management;

public class FirstStart : MonoBehaviour
{
    public static Action<bool> ReadyToStart;
    public static Action<bool> Conected;
    public static Action Error;

    [SerializeField] int TimeOut = 0;

    void Awake()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier + "\n");
        Debug.Log(SystemInfo.deviceModel + "\n");
        Debug.Log(SystemInfo.deviceName + "\n");
        Debug.Log(SystemInfo.graphicsDeviceID + "\n");
        Debug.Log(SystemInfo.graphicsDeviceName + "\n");
        Debug.Log(SystemInfo.systemMemorySize + "\n");

        if (PlayerPrefs.HasKey("SoundValue"))
        {
            Debug.Log("1");
            SendUserConfig();
        }
    }

    void SendUserConfig()
    {
        Debug.Log("2");
        StartCoroutine("POST");

        //SystemInfo.deviceUniqueIdentifierц3
    }

    public IEnumerator POST()
    {
        Debug.Log("3");
        var URL = "https://wingift.cf/api/devices/create?device=" + SystemInfo.deviceUniqueIdentifier;

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            webRequest.timeout = TimeOut;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
                Error();
            }

            if(webRequest.isHttpError)
            {
                Debug.Log(webRequest.responseCode);
                Error();
            }

            if(webRequest.responseCode == 500)
            {
                Debug.Log(webRequest.isHttpError);
            }
            
            else
            {
                var response = webRequest.downloadHandler.text;
                //JsonUtility.FromJson<>

                //TODO: додлей, чтобы ошибки нормально читало и в консоль выодило
                Debug.Log(response);
            }
        }
    }
}
