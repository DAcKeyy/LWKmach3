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


public class AuthorizationProcessing : MonoBehaviour
{
    [SerializeField] GameObject PasswordErrorSign = null;
    [SerializeField] GameObject EmailErrorSign = null;
    [SerializeField] TMP_InputField EmailField = null;
    [SerializeField] TMP_InputField PasswordField = null;
    [SerializeField] TMP_Text TextUponFields = null;
    [SerializeField] GameObject BigFuckingPanel = null;
    [SerializeField] LevelLoader levelLoader = null;
    [SerializeField] Image LoadIndicator = null;

    WebSender Sender = new WebSender();
    RegistrationFiledsCheker FieldsCheker = new RegistrationFiledsCheker();
    ServerLoadProcess ServerLoading = new ServerLoadProcess();
    ErrorTypeCheker errorTypeCheker = new ErrorTypeCheker();

    public static Action FirstStartUp;

    private void OnEnable()
    {
        AuthorizationButtonClicked.AccButtPres += StartRqesting;

        ServerLoading.loadIndicator = LoadIndicator;
    }

    private void OnDisable()
    {
        AuthorizationButtonClicked.AccButtPres -= StartRqesting;
    }

    private void Start()
    {
        FieldsCheker.ErrorSignEmail = EmailErrorSign;
        FieldsCheker.ErrorSignPassword = PasswordErrorSign;
        FieldsCheker.DescriptionText = TextUponFields;

        

        if (PlayerPrefs.HasKey("Token"))
        {
            GlobalDataBase.Email = PlayerPrefs.GetString("Email");
            GlobalDataBase.Password = PlayerPrefs.GetString("Password");
            GlobalDataBase.Token = PlayerPrefs.GetString("Token");

            AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);

            var webRequest = UnityWebRequest.Post(URLStruct.Authorization, AuthForm.Form);

            StartCoroutine(Sender.POST(webRequest, Authorization, Errors));
            StartCoroutine(ServerLoading.LoadAsynchronously(webRequest));
        }

        else 
        {
            FirstStartUp();
        }
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

            StartCoroutine(Sender.POST(webRequest, Authorization, Errors));
            StartCoroutine(ServerLoading.LoadAsynchronously(webRequest));

        }

        if(type == 4) //Registration
        {
            RegistartionForm RegForm = new RegistartionForm(GlobalDataBase.Email, GlobalDataBase.Password);

            var webRequest = UnityWebRequest.Post(URLStruct.Registration, RegForm.Form);

            StartCoroutine(Sender.POST(webRequest, Registration, Errors));
            StartCoroutine(ServerLoading.LoadAsynchronously(webRequest));
        }
    }

    void Authorization(string Response)
    {
        var Objcet = JsonUtility.FromJson<GetToken>(Response);

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

    void Errors(string Response)
    {
        Debug.Log(Response);
        var ErrorObjcet = JsonUtility.FromJson<ErrorResponse>(Response);

        if(String.IsNullOrEmpty(ErrorObjcet.errors[0].detail))
            TextUponFields.text = ErrorObjcet.errors[0].title;
        else TextUponFields.text = ErrorObjcet.errors[0].detail;

        if (errorTypeCheker.CheckType(ErrorObjcet.errors[0].title, ErrorObjcet.errors[0].detail) == ErrorType.EmailIsExist)
            EmailErrorSign.SetActive(true);

        if (errorTypeCheker.CheckType(ErrorObjcet.errors[0].title, ErrorObjcet.errors[0].detail) == ErrorType.VerifyEmail)
        {
            BigFuckingPanel.SetActive(true);
            var text = BigFuckingPanel.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "Verify account on your email";
        }           
    }
}

public class ErrorTypeCheker
{
    public ErrorType CheckType(string title, string detail)
    {
        Regex emailIsExist = new Regex(@"The email has already been taken.");
        Regex error500 = new Regex(@"Internal Server Error");
        Regex verifyEmail = new Regex(@"Your email address is not verified.");

        if (String.IsNullOrEmpty(detail) == false && emailIsExist.IsMatch(detail))
            return ErrorType.EmailIsExist;

        if (error500.IsMatch(detail))
            return ErrorType.Error500;

        if (String.IsNullOrEmpty(detail) == false && verifyEmail.IsMatch(detail))
            return ErrorType.VerifyEmail;

        return ErrorType.None;
    }

}

public class ServerLoadProcess
{
    public Image loadIndicator;
    private float fillValue = 0.02f;

    public IEnumerator LoadAsynchronously(UnityWebRequest webRequest)
    {
        while (!webRequest.isDone)
        {
            loadIndicator.fillAmount += fillValue;

            if(loadIndicator.fillAmount == 1f)
            {
                loadIndicator.fillClockwise = !loadIndicator.fillClockwise;
                fillValue *= -1;
            }

            if(loadIndicator.fillAmount == 0f)
            {
                loadIndicator.fillClockwise = !loadIndicator.fillClockwise;
                fillValue *= -1;
            }

            yield return null;
        }
    }
}


