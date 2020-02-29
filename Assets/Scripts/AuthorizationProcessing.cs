using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;
using System.Text.RegularExpressions;


public enum ErrorType
{
    None,
    Error500,
    VerifyEmail,
    EmailIsExist,
}

public enum ResponseType
{


}



public class AuthorizationProcessing : MonoBehaviour
{
    [SerializeField] GameObject UserPanel;
    [SerializeField] private GameObject CouponsPanel = null;
    [SerializeField] private GameObject AccountAcceptedPanel = null;
    [SerializeField] private GameObject AccountRequestPanel = null;
    [SerializeField] private GameObject PasswordErrorSign = null;
    [SerializeField] private GameObject EmailErrorSign = null;
    [SerializeField] private TMP_InputField EmailField = null;
    [SerializeField] private TMP_InputField PasswordField = null;
    [SerializeField] private TMP_Text TextUponFields = null;
    [SerializeField] private GameObject PanelWithText = null;
    [SerializeField] private LevelLoader levelLoader = null;
    [SerializeField] private ServerLoadingProcess LoadIndicator = null;


    RegistrationFiledsCheker FieldsCheker = new RegistrationFiledsCheker();
    WebSender Sender = new WebSender();


    private void OnEnable()
    {
        AuthorizationButtonClicked.AccButtPres += StartRqesting;
    }

    private void OnDisable()
    {
        AuthorizationButtonClicked.AccButtPres -= StartRqesting;
    }

    private void Start()
    {
        UserPanel.SetActive(false);                 //чтобы не заморачиваться с выключением панелей в едиторе
        AccountAcceptedPanel.SetActive(false);
        AccountRequestPanel.SetActive(false);
        CouponsPanel.SetActive(false);

        var webRequest = UnityWebRequest.Get("http://google.com");
        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");

        StartCoroutine(Sender.SendWebRequest(webRequest, IfNetworkOk, ConectionError));
        StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));

        FieldsCheker.ErrorSignEmail = EmailErrorSign;
        FieldsCheker.ErrorSignPassword = PasswordErrorSign;
        FieldsCheker.DescriptionText = TextUponFields;
    }

    void StartRqesting(byte type)
    {
        if (!FieldsCheker.CheckFields(EmailField, PasswordField))
        {
            Debug.Log("ПРОВЕРЬ ПОЛЯ ВВОДА");

            return;
        }

        else
        {
            GlobalDataBase.Email = EmailField.text;
            GlobalDataBase.Password = PasswordField.text;
        }


        if (type == 5) //Autharization
        {
            AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);

            Debug.Log("ADSAD");

            var webRequest = UnityWebRequest.Post(URLStruct.Authorization, AuthForm.Form);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");

            StartCoroutine(Sender.SendWebRequest(webRequest, Authorization, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));

        }

        if (type == 4) //Registration
        {
            RegistartionForm RegForm = new RegistartionForm(GlobalDataBase.Email, GlobalDataBase.Password);

            var webRequest = UnityWebRequest.Post(URLStruct.Registration, RegForm.Form);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            StartCoroutine(Sender.SendWebRequest(webRequest, Registration, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }
    }

    void Authorization(string response)
    {
        var Objcet = JsonUtility.FromJson<GetToken>(response);

        GlobalDataBase.Token = Objcet.access_token;

        PlayerPrefs.SetString("Token", Objcet.access_token);
        PlayerPrefs.SetString("Refresh_Token", Objcet.refresh_token);
        PlayerPrefs.SetString("Password", GlobalDataBase.Password);
        PlayerPrefs.SetString("Email", GlobalDataBase.Email);

        var webRequest = UnityWebRequest.Get(URLStruct.GetCoin);
        webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);
        StartCoroutine(Sender.SendWebRequest(webRequest, LoadLevel, Error));
        StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
    }

    void Registration(string response)
    {
        Debug.Log(response);
    }

    void LoadLevel(string response)
    {
        var Obj = JsonUtility.FromJson<Me>(response);
        GlobalDataBase.Gold = Convert.ToInt32(Obj.data.attributes.coin);

        levelLoader.LoadLevel("Menu");
    }

    void Error(string response)
    {
        Debug.Log(response);
    }

    void ConectionError(string response)
    {
        if (String.IsNullOrEmpty(response))
        {
            CheckForFirstStartup();
        }
        else NoConection();
    }

    void Errors(string response)
    {
        Debug.Log(response);

        Regex emailIsExist = new Regex(@"The email has already been taken.");
        Regex error500 = new Regex(@"Internal Server Error");
        Regex verifyEmail = new Regex(@"Your email address is not verified");
        Regex WrongFiledsData = new Regex(@"The provided authorization grant");

        if (String.IsNullOrEmpty(response) == false && emailIsExist.IsMatch(response))
        {
            EmailErrorSign.SetActive(true);
            TextUponFields.text = "The email has already been taken";
        }

        if (error500.IsMatch(response))
        {
            AccountAcceptedPanel.SetActive(false);
            AccountRequestPanel.SetActive(false);
            CouponsPanel.SetActive(false);

            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "Server doesn't response";
        }

        if (String.IsNullOrEmpty(response) == false && verifyEmail.IsMatch(response))
        {
            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "Verify account on your email";
        }

        if(WrongFiledsData.IsMatch(response))
        {
            EmailErrorSign.SetActive(true);
            PasswordErrorSign.SetActive(true);
            TextUponFields.text = "Wrong email or password";
        }
    }

    void CheckForFirstStartup()
    {
        if (PlayerPrefs.HasKey("Token"))
        {
            GlobalDataBase.Email = PlayerPrefs.GetString("Email");
            GlobalDataBase.Password = PlayerPrefs.GetString("Password");
            GlobalDataBase.Token = PlayerPrefs.GetString("Token");

            AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);

            var webRequest = UnityWebRequest.Post(URLStruct.Authorization, AuthForm.Form);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            StartCoroutine(Sender.SendWebRequest(webRequest, Authorization, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }
        else FirstStartUp();
    }

    void FirstStartUp()
    {
        Debug.Log("FirstStartUp");
        UserPanel.SetActive(true);
        AccountRequestPanel.SetActive(true);
    }
    void NoConection()
    {
        Debug.Log("NoConection");
        UserPanel.SetActive(true);
        PanelWithText.SetActive(true);
        CouponsPanel.SetActive(true);
        PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "No internet Conection";
    }

    void IfNetworkOk(string response)
    {
        CheckForFirstStartup();
    }
}