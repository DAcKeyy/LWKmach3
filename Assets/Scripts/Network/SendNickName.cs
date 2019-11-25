using UnityEngine;
using TMPro;

public class SendNickName : MonoBehaviour
{
    [SerializeField] GameObject ThirdPanel = null;
    [SerializeField] GameObject TextField = null;
    [SerializeField] GameObject AccRequestPan = null;
    [SerializeField] GameObject NicknamePanel = null;
    [SerializeField] GameObject EventSystem = null;

    JSON_Controller JS = null;

    private void Start()
    {
        JS = EventSystem.GetComponent<JSON_Controller>();
    }

    public void Send()
    {
        if (6 < TextField.GetComponent<TMP_InputField>().text.Length && TextField.GetComponent<TMP_InputField>().text.Length < 32)
        {
            GlobalDataBase.NickName = TextField.GetComponent<TMP_InputField>().text;

            JS.StartPOST_JSON(0);

            AccRequestPan.SetActive(false);
            NicknamePanel.SetActive(false);
        }
        else ThirdPanel.SetActive(true);       
    }
}
