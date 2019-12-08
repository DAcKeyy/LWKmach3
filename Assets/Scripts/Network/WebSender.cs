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

    public IEnumerator POST(string URL, Dictionary<string, string> form , Action<string> Response)
    {
        Response = delegate { };

        Debug.Log(form);


        using (var webRequest = UnityWebRequest.Post(URL, form))
        {
            //Debug.Log(form.Values + "///" + form.Keys.Count  +"///" + form.Keys); vnd.api+json

            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");

            //webRequest.uploadHandler.contentType = "application/vnd.api+json";

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error + "<b>///<b>" + webRequest.downloadHandler.text);
                Response(webRequest.error);
                
            }

            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                Response(webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator GET(string URL, Action<string> Response)
    {
        Response = delegate { };

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                Response(webRequest.error);
            }

            else
            {
                Response(webRequest.downloadHandler.text);
            }
        }
    }
}
