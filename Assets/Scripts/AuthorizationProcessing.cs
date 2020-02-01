﻿using UnityEngine;
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


public class AuthorizationProcessing : MonoBehaviour
{
    [SerializeField] GameObject UserPanel = null;
    [SerializeField] GameObject CouponsPanel = null;
    [SerializeField] GameObject AccountAcceptedPanel = null;
    [SerializeField] GameObject AccountRequestPanel = null;
    [SerializeField] GameObject PasswordErrorSign = null;
    [SerializeField] GameObject EmailErrorSign = null;
    [SerializeField] TMP_InputField EmailField = null;
    [SerializeField] TMP_InputField PasswordField = null;
    [SerializeField] TMP_Text TextUponFields = null;
    [SerializeField] GameObject PanelWithText = null;
    [SerializeField] LevelLoader levelLoader = null;
    [SerializeField] ServerLoadingProcess LoadIndicator = null;


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
        if (FieldsCheker.CheckFields(EmailField, PasswordField) == false)
        {
            Debug.Log("поля ввода проверь, сын бляди");

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

    void Authorization(string Response)
    {
        var Objcet = JsonUtility.FromJson<GetToken>(Response);

        GlobalDataBase.Token = Objcet.access_token;

        PlayerPrefs.SetString("Token", Objcet.access_token);
        PlayerPrefs.SetString("Refresh_Token", Objcet.refresh_token);
        PlayerPrefs.SetString("Password", GlobalDataBase.Password);
        PlayerPrefs.SetString("Email", GlobalDataBase.Email);

        levelLoader.LoadLevel("Menu");
    }

    void Registration(string Response)
    {
        Debug.Log(Response);
    }



    void ConectionError(string Response)
    {
        if (String.IsNullOrEmpty(Response))
        {
            CheckForFirstStartup();
        }
        else NoConection();
    }

    void Errors(string Response)
    {
        Debug.Log(Response);


        Regex emailIsExist = new Regex(@"The email has already been taken.");
        Regex error500 = new Regex(@"Internal Server Error");
        Regex verifyEmail = new Regex(@"Your email address is not verified");
        Regex WrongFiledsData = new Regex(@"The provided authorization grant");

        if (String.IsNullOrEmpty(Response) == false && emailIsExist.IsMatch(Response))
        {
            EmailErrorSign.SetActive(true);
            TextUponFields.text = "The email has already been taken";
        }

        if (error500.IsMatch(Response))
        {
            AccountAcceptedPanel.SetActive(false);
            AccountRequestPanel.SetActive(false);
            CouponsPanel.SetActive(false);

            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "Server doesn't response";
        }

        if (String.IsNullOrEmpty(Response) == false && verifyEmail.IsMatch(Response))
        {
            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "Verify account on your email";
        }

        if(WrongFiledsData.IsMatch(Response))
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

    void IfNetworkOk(string Response)
    {
        CheckForFirstStartup();
    }
}