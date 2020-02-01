using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] GameObject CanvasGameObject = null;
    [SerializeField] GameObject ErrorPanel = null;
    [SerializeField] TMP_Text textToGold = null;
 
    WebSender Sender = new WebSender();

    private void OnEnable()
    {
        TaskManager.TaskComplete += SetText;
    }

    private void OnDisable()
    {
        TaskManager.TaskComplete -= SetText;
    }

    void SetText()
    {
        textToGold.text = (GlobalDataBase.Gold + 1).ToString();

        ToIncreaseForm IncreaseForm = new ToIncreaseForm("1");

        var webRequest = UnityWebRequest.Post(URLStruct.SendCoins, IncreaseForm.Form);

        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Content-Type", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);

        StartCoroutine(Sender.SendWebRequest(webRequest, Response, Error));
    }


    void Response(string response)
    {
        Debug.Log("бабло доставлено");
    }

    void Error(string response)
    {
        CanvasGameObject.SetActive(true);
        ErrorPanel.SetActive(true);
        var text = ErrorPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        text.text = response;
    }
}

