using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Text.RegularExpressions;
using LWT.System;
using Zenject;
using ModestTree;

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

namespace LWT.Networking
{
    public class AuthorizationProcessing : MonoBehaviour//жутко перегруженный класс, да простят меня боги ооп
    {
        [Inject]
        private StartSceneInputHandels inputHandles = null;

        [SerializeField] private GameObject ErrorPanel = null;
        [SerializeField] private GameObject UserPanel = null;
        [SerializeField] private GameObject CouponsPanel = null;
        [SerializeField] private GameObject AccountAcceptedPanel = null;
        [SerializeField] private GameObject AccountRequestPanel = null;
        [SerializeField] private GameObject AuthPasswordErrorSign = null;
        [SerializeField] private GameObject AuthEmailErrorSign = null;
        [SerializeField] private GameObject RegistPasswordErrorSign = null;
        [SerializeField] private GameObject RegistEmailErrorSign = null;
        [SerializeField] private TMP_InputField AuthEmailField = null;
        [SerializeField] private TMP_InputField AuthPasswordField = null;
        [SerializeField] private TMP_InputField RegistEmailField = null;
        [SerializeField] private TMP_InputField RegistPasswordField = null;
        [SerializeField] private TMP_InputField RestorePassEmailField = null;
        [SerializeField] private TMP_Text TextUponFields = null;
        [SerializeField] private GameObject PanelWithText = null;
        [SerializeField] private LevelLoader levelLoader = null;
        [SerializeField] private ServerLoadingProcess LoadIndicator = null;

        RegistrationFiledsCheker FieldsCheker = new RegistrationFiledsCheker();
        WebSender Sender = new WebSender();

        private void Start()
        {
            CheckInternerConection();

            CheckForServerErrors();





            //UserPanel.SetActive(false);                 //чтобы не заморачиваться с выключением панелей в едиторе
            //AccountAcceptedPanel.SetActive(false);
            //AccountRequestPanel.SetActive(false);
            //CouponsPanel.SetActive(false);

            //CheckInternerConection();

            //inputHandles.LoginClick += Authorization;
            //inputHandles.RegistrationClick += Registration;
            //inputHandles.RestorePasswordClick += ResetPassword;
            //inputHandles.ResendEmailClick += ResendEmail;

            //FieldsCheker.ErrorSignEmail = AuthPasswordErrorSign;
            //FieldsCheker.ErrorSignPassword = AuthEmailErrorSign;
            //FieldsCheker.DescriptionText = TextUponFields;
        }


        void CheckInternerConection()
        {
            var webRequest = UnityWebRequest.Get("https://www.google.com/");

            StartCoroutine(Sender.SendWebRequest(webRequest, CheckInternerConectionResponse, ConectionError));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void CheckForCoupons()
        {
            var webRequest = UnityWebRequest.Get("https://api.sarond.cf/availability");

            StartCoroutine(Sender.SendWebRequest(webRequest, CheckForCouponsResponse, ConectionError));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void CheckForCouponsResponse(string json)
        {
            var response = JsonUtility.FromJson<Avability>(json);
            if (response.availability == GlobalDataBase.TrueString)
            {
                if (String.IsNullOrEmpty(GlobalDataBase.Error))
                    LoadLevel(null);
            }
            else BlockPanel("The game has closed due to the lack of coupons in the system");


        }

        void CheckForServerErrors()
        {
            if(!String.IsNullOrEmpty(GlobalDataBase.Error))
            {
                BlockPanel(GlobalDataBase.Error);


            }
        }

        void BlockPanel(string text)
        {
            ErrorPanel.SetActive(true);
            ErrorPanel.GetComponentInChildren<TMP_Text>().text = text;
        }

        void Authorization()
        {
            if(!PlayerPrefs.HasKey("Token"))
            {
                if (!FieldsCheker.CheckFields(AuthEmailField, AuthPasswordField))
                {
                    Debug.Log("ПРОВЕРЬ ПОЛЯ ВВОДА");

                    return;
                }

                else
                {
                    GlobalDataBase.Email = AuthEmailField.text;
                    GlobalDataBase.Password = AuthPasswordField.text;
                }
            }

            AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);
            var webRequest = UnityWebRequest.Post(URLStruct.Authorization, AuthForm.Form);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            StartCoroutine(Sender.SendWebRequest(webRequest, AuthorizationResponse, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }
        void AuthorizationResponse(string response)
        {
            var Objcet = JsonUtility.FromJson<GetToken>(response);

            GlobalDataBase.Token = Objcet.access_token;

            PlayerPrefs.SetString("Token", Objcet.access_token);
            PlayerPrefs.SetString("Refresh_Token", Objcet.refresh_token);
            PlayerPrefs.SetString("Password", GlobalDataBase.Password);
            PlayerPrefs.SetString("Email", GlobalDataBase.Email);

            GetAccountInfo();
        }
        void Registration()
        {
            if (!FieldsCheker.CheckFields(RegistEmailField, RegistPasswordField))
            {
                Debug.Log("ПРОВЕРЬ ПОЛЯ ВВОДА");

                return;
            }

            else
            {
                RegistartionForm RegForm = new RegistartionForm(RegistEmailField.text, RegistPasswordField.text);
                var webRequest = UnityWebRequest.Post(URLStruct.Registration, RegForm.Form);
                webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
                StartCoroutine(Sender.SendWebRequest(webRequest, RegistrationResponse, Errors));
                StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
            }
        }

        void RegistrationResponse(string response)
        {
            PanelWithText.SetActive(true);
            PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "Verify account on your email";
        }

        void ResetPassword()
        {
            var webRequest = UnityWebRequest.Post(URLStruct.ResetPassword + "?email=" + RestorePassEmailField.text, "");
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            webRequest.SetRequestHeader("Content-Type", "application/vnd.api+json");
            StartCoroutine(Sender.SendWebRequest(webRequest, ResetPasswordResponse, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void ResetPasswordResponse(string response)
        {
            PanelWithText.SetActive(true);
            var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
            text.text = "A password reset request has been sent to your mail";
        }

        void ResendEmail()
        {
            var webRequest = UnityWebRequest.Post(URLStruct.ResendEmail + "?email=" + RegistEmailField.text, "");
            StartCoroutine(Sender.SendWebRequest(webRequest, ResendEmailResponse, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void ResendEmailResponse(string response)
        {
            PanelWithText.SetActive(true);
            PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "Check your email";
        }

        void GetAccountInfo()
        {            
            var webRequest = UnityWebRequest.Get(URLStruct.GetAccountInfo);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);
            StartCoroutine(Sender.SendWebRequest(webRequest, LoadLevel, Error));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void LoadLevel(string response)
        {
            //var Obj = JsonUtility.FromJson<Me>(response);

            //URLStruct.CouponsLink = Obj.data.links.self;

            levelLoader.LoadScene("Menu");
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

            Regex emailIsExist = new Regex(@"The email has already been taken");
            Regex error500 = new Regex(@"Internal Server Error");
            Regex verifyEmail = new Regex(@"Your email address is not verified");
            Regex WrongFiledsData = new Regex(@"Incorrect username or password");
            Regex ResetPass = new Regex(@"Не удалось найти пользователя с указанным электронным адресом");


            if (String.IsNullOrEmpty(response) == false && emailIsExist.IsMatch(response))
            {
                PanelWithText.SetActive(true);
                var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
                text.text = "The email has already been taken";
                RegistEmailErrorSign.SetActive(true);
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

            if (WrongFiledsData.IsMatch(response))
            {
                AuthEmailErrorSign.SetActive(true);
                AuthPasswordErrorSign.SetActive(true);
                TextUponFields.text = "Wrong email or password";
            }

            if (ResetPass.IsMatch(response))
            {
                PanelWithText.SetActive(true);
                var text = PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
                text.text = "A password reset request has been sent to your mail";
            }
        }

        void CheckForFirstStartup()
        {
            if (PlayerPrefs.HasKey("Token"))
            {
                GlobalDataBase.Email = PlayerPrefs.GetString("Email");
                GlobalDataBase.Password = PlayerPrefs.GetString("Password");
                GlobalDataBase.Token = PlayerPrefs.GetString("Token");

                Authorization();
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
            PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "No internet Connection";
        }

        void CheckInternerConectionResponse(string response)
        {
            CheckForCoupons();
        }
    }
}