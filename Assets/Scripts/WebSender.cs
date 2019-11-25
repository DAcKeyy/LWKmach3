using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public enum TypeRequest
{
    Get,
    Post
}

public class WebSender
{
    public Action<string> Response;

    public IEnumerator POST(string URL, Action<string> Response)
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
