using System;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using TMPro;

public class Test_CreateCoupon : MonoBehaviour
{
    [SerializeField] TMP_InputField Company = null,
                                    Promo = null,
                                    Description = null,
                                    Life_Spain = null,
                                    Contact = null;

    public void Send()
    {
        StartCoroutine("POST", "https://wingift.cf/api/promo/create?promo=" + Promo.text 
            + "&company_name=" + Company.text 
            + "&description=" + Description.text 
            + "&contact=" + Contact.text 
            + "&expiration_date=" + Life_Spain.text);
    }

    public IEnumerator POST(string URL)
    {
        

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
        {
            Debug.Log(webRequest.url);
           
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
