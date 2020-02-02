using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] GameObject CanvasGameObject = null;
    [SerializeField] GameObject ErrorPanel = null;
    [SerializeField] TMP_Text textToGold = null;

    private int GoldInRound;
 
    WebSender Sender = new WebSender();

    private void OnEnable()
    {
        TaskManager.TaskComplete += SetText;
    }

    private void OnDisable()
    {
        TaskManager.TaskComplete -= SetText;
    }

    private void Start()
    {
        textToGold.text = GlobalDataBase.Gold.ToString();
        GoldInRound = GlobalDataBase.Gold;
    }

    void SetText()
    {
        GoldInRound += 1;

        textToGold.text = GoldInRound.ToString();

        ToIncreaseForm IncreaseForm = new ToIncreaseForm("1");

        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"increase\":\"1\"}");

        var webRequest = new UnityWebRequest(URLStruct.SendCoins, "POST");

        webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw (bodyRaw);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Content-Type", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);

        StartCoroutine(Sender.SendWebRequest(webRequest, Response, Error));
    }


    void Response(string response)
    {
        Debug.Log("бабло доставлено");
        GlobalDataBase.Gold = GoldInRound;
    }

    void Error(string response)
    {
        CanvasGameObject.SetActive(true);
        ErrorPanel.SetActive(true);
        var text = ErrorPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        text.text = response;
    }
}

