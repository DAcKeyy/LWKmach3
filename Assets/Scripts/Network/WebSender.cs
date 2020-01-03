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
        Debug.Log("???????");
        Debug.Log(webRequest.method);

        webRequest.SetRequestHeader("Accept", "application/vnd.api+json"); //vnd.api+json

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