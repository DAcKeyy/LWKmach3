using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TokenCheker : MonoBehaviour
{
    #region Field
    [SerializeField] GameObject DialogPanel = null;
    //[SerializeField] GameObject EventSystem = null;
    [SerializeField] GameObject BlockPanel = null;

    [Header("For Usual Aut")]
    [SerializeField] GameObject LoginField = null;
    [SerializeField] GameObject PasswordField = null;

    //private string URL = null;
    //private JSON_Controller JS = null;
    private GameObject ExclamationMarkForLogin = null;
    private GameObject ExclamationMarkForPassword = null;
    private string FirstPass;


    //URLSpliter URLspliter = new URLSpliter();
    #endregion

    private void OnEnable()
    {
        //JS = EventSystem.GetComponent<JSON_Controller>();

        AuthorizationButtonClicked.AccButtPres += Autrisation;

        ExclamationMarkForLogin = DialogPanel.transform.Find("Cool Stuff Panel").transform.Find("Panel 1").gameObject;
        ExclamationMarkForPassword = DialogPanel.transform.Find("Cool Stuff Panel").transform.Find("Panel 2").gameObject;
    }

    private void FixedUpdate()
    {   

    }

    private void Autrisation(byte type)
    {
        if (type == 0)
        {

        }

        if (type == 1)
        {

        }

        if (type == 2)
        {

        }

        if (type == 3)
        {

        }

        if (type == 4)//блять пиздец ебать я тут кода написал
        {
            if (LoginField.GetComponent<TMP_InputField>().text.Length > 0)
            {
                if (PasswordField.GetComponent<TMP_InputField>().text.Length >= 6)
                {
                    if (string.IsNullOrEmpty(FirstPass))
                    {
                        FirstPass = PasswordField.GetComponent<TMP_InputField>().text;

                        PasswordField.GetComponent<TMP_InputField>().text = ("");

                        var Placeholder = PasswordField.transform.Find("Text Area").transform.Find("Placeholder").gameObject;
                        Placeholder.GetComponent<TMP_Text>().text = ("Enter password again...");

                        ExclamationMarkForPassword.SetActive(true);
                    }
                    else
                    {
                        if (FirstPass == PasswordField.GetComponent<TMP_InputField>().text)
                        {
                            //GlobalDataBase.Email = LoginField.GetComponent<TMP_InputField>().text;
                            //GlobalDataBase.Password = PasswordField.GetComponent<TMP_InputField>().text;

                            DialogPanel.transform.Find("Nickname Panel").gameObject.SetActive(true);
                        }
                        else
                        {
                            ExclamationMarkForPassword.SetActive(true);
                            var Placeholder = PasswordField.transform.Find("Text Area").transform.Find("Placeholder").gameObject;
                            Placeholder.GetComponent<TMP_Text>().text = ("Incorrect password...");
                        }
                    }
                }
                else if (PasswordField.GetComponent<TMP_InputField>().text.Length > 0)
                {
                    var PassPanel = DialogPanel.transform.Find("Password Panel").gameObject;
                    PassPanel.SetActive(true);
                }
                else ExclamationMarkForPassword.SetActive(true);
            }
            else ExclamationMarkForLogin.SetActive(true);
        }

        if (type == 5)
        {
            if (LoginField.GetComponent<TMP_InputField>().text.Length > 0)
            {
                //GlobalDataBase.Email = LoginField.GetComponent<TMP_InputField>().text;

                if (PasswordField.GetComponent<TMP_InputField>().text.Length > 0)
                {
                    //GlobalDataBase.Password = PasswordField.GetComponent<TMP_InputField>().text;

                    //JS.StartPOST_JSON(1);

                    BlockPanel.SetActive(true);
                }
                else ExclamationMarkForPassword.SetActive(true);
            }
            else ExclamationMarkForLogin.SetActive(true);
        }
    }
}
