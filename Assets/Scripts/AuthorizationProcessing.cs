using UnityEngine;

public class AuthorizationProcessing : MonoBehaviour
{
    WebSender Sender = new WebSender();

    private void Start()
    {
        if(PlayerPrefs.HasKey("Token"))
        {
            StartCoroutine(Sender.POST(URLStruct.Registration, Authorization));
        }
        else
        {
            StartCoroutine(Sender.POST(URLStruct.Registration, Registration));
        }
    }

    void Registration(string Response)
    {
        Debug.Log(Response);
    }

    void Authorization(string Response)
    {

    }
}