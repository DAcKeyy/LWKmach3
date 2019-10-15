using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FirstStart : MonoBehaviour
{
    void Awake()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        if (!PlayerPrefs.HasKey("SoundValue"))
        {
            SendMAC();
        }
    }

    void SendMAC()
    {
        StartCoroutine("POST");

        //SystemInfo.deviceUniqueIdentifierц3
    }

    public IEnumerator POST()
    {
        var URL = "https://wingift.cf/api/devices/create";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                var response = webRequest.downloadHandler.text;
                Debug.Log(response);
            }
        }
    }
}
