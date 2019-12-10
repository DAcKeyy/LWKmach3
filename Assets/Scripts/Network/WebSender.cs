using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TypeRequest
{
    Get,
    Post
}

public class WebSender 
{
    public Action<string> Response;
    public Action<string> Error;

    public IEnumerator POST(string URL, Dictionary<string, string> form , Action<string> Response, Action<string> Error)
    {
        using (var webRequest = UnityWebRequest.Post(URL, form))
        {

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

    public IEnumerator GET<T>(string URL, Action<string> Response, Action<string> Error)
    {
        Response = delegate { };

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
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
}
