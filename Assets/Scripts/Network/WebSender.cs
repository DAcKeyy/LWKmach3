using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;

public class WebSender 
{
    public Action<string> Response;
    public Action<string> Error;

    public IEnumerator SendWebRequest(UnityWebRequest webRequest, Action<string> Response, Action<string> Error)
    {
        Debug.Log(webRequest.method);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            if (String.IsNullOrEmpty(webRequest.downloadHandler.text))
                Error(webRequest.error);
            else Error(webRequest.downloadHandler.text);
        }

        else
        {
            Response(webRequest.downloadHandler.text);
        }
    }
}