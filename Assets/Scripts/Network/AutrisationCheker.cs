using UnityEngine;
using System;

public class AutrisationCheker : MonoBehaviour
{

    [SerializeField] GameObject DialogPanel = null;
    [SerializeField] GameObject EventSystem = null;
    [SerializeField] GameObject AutorisationPanel = null;
    [SerializeField] GameObject AccountPanel = null;
    [SerializeField] GameObject AccRequestPan = null;
    [SerializeField] GameObject BlockPanel = null;
    private void OnEnable()
    {
        JSON_Controller.RegistrationErrors += CheckRegistrtion;
        JSON_Controller.LoginErrors += CheckLogin;
        JSON_Controller.GetUserDataErrors += GetUserData;
    }

    void Start()
    {
        JSON_Controller JS = EventSystem.GetComponent<JSON_Controller>();

        if (!PlayerPrefs.HasKey("UsersToken"))
        {
            AutorisationPanel.SetActive(true);
        }
        else
        {
            if (string.IsNullOrEmpty(GlobalDataBase.Token))
            {
                AutorisationPanel.SetActive(true);
            }
            else
            {
                

                JS.StartGET_JSON(0);
            }
        }
    }

    void CheckRegistrtion(string error)
    {
        if(string.IsNullOrEmpty(error))
        {
            BlockPanel.SetActive(false);
            DialogPanel.transform.Find("Cool Stuff Panel").transform.Find("Panel CheckEmail").gameObject.SetActive(true);
            AutorisationPanel.SetActive(true);
        }
        else if (!string.IsNullOrEmpty(error))
        {
            BlockPanel.SetActive(false);
            DialogPanel.transform.Find("Error Panel").transform.Find("Error:Unknown Panel").gameObject.SetActive(true);
        }
    }

    void CheckLogin(string error)
    {
        JSON_Controller JS = EventSystem.GetComponent<JSON_Controller>();

        if (string.IsNullOrEmpty(error))
        {
            AutorisationPanel.SetActive(false);
            BlockPanel.SetActive(false);
            JS.StartGET_JSON(0);
        }
        else if(error == "Unauthorized")
        {
            BlockPanel.SetActive(false);
            DialogPanel.transform.Find("Error Panel").transform.Find("Error:Unauthorised Panel").gameObject.SetActive(true);
        }
        else if(!string.IsNullOrEmpty(error))
        {
            BlockPanel.SetActive(false);
            DialogPanel.transform.Find("Error Panel").transform.Find("Error:Unknown Panel").gameObject.SetActive(true);
        }
    }

    void GetUserData(string error)
    {
        if(string.IsNullOrEmpty(error))
        {
            AccRequestPan.SetActive(false);

            AccountPanel.SetActive(true);
        }
        else
        {
            DialogPanel.transform.Find("Error Panel").transform.Find("Error:Unknown Panel").gameObject.SetActive(true);
        }
    }
}
