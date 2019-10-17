using System;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using TMPro;

public class Test_CreateCoupon : MonoBehaviour
{
    [SerializeField] TMP_InputField Company, Promo, Description, Life_Spain, Contact = null;

    public void Send()
    {
        StartCoroutine("POST","https://wingift.cf/api/promo/create?");
    }

    public IEnumerator POST(string URL)
    {
        

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            webRequest.SetRequestHeader("promo", Promo.text);
            webRequest.SetRequestHeader("company_name", Company.text);
            webRequest.SetRequestHeader("description", Description.text);
            webRequest.SetRequestHeader("contact", Contact.text);
            webRequest.SetRequestHeader("expiration_date", Life_Spain.text);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
