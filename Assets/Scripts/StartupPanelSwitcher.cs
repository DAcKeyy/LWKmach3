using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class StartupPanelSwitcher : MonoBehaviour
{
    [SerializeField] GameObject UserPanel = null;
    [SerializeField] GameObject AccountAcceptedPanel = null;
    [SerializeField] GameObject AccountRequestPanel = null;
    [SerializeField] GameObject NoConectionPanel = null;

    private void OnEnable()
    {
        UserPanel.SetActive(false);                 //чтобы не заморачиваться с выключением панелей в едиторе
        AccountAcceptedPanel.SetActive(false);
        AccountRequestPanel.SetActive(false);

        StartCoroutine(checkInternetConnection(NoConection));

        

        AuthorizationProcessing.FirstStartUp += FirstStartUp;
    }

    private void OnDisable()
    {
        AuthorizationProcessing.FirstStartUp -= FirstStartUp;
    }

    void NoConection()
    {
        Debug.Log("NoConection");
        NoConectionPanel.SetActive(true);
        NoConectionPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "No internet Conection";
    }

    void FirstStartUp()
    {
        Debug.Log("FirstStartUp");
        UserPanel.SetActive(true);
        AccountRequestPanel.SetActive(true);
    }

    IEnumerator checkInternetConnection(Action NoConetion)
    {
        var www = new UnityEngine.Networking.UnityWebRequest("http://google.com");
        yield return www;
        if (www.error != null)
        {
            NoConetion();
        }

    }
}
