using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Text.RegularExpressions;
using LWT.System;
using Zenject;

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
    public class AuthorizationProcessing : MonoBehaviour
    {
        [Inject]
        private StartSceneInputHandels inputHandles = null;

        [SerializeField] private GameObject UserPanel = null;
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

        private void Start()
        {
            UserPanel.SetActive(false);                 //чтобы не заморачиваться с выключением панелей в едиторе
            AccountAcceptedPanel.SetActive(false);
            AccountRequestPanel.SetActive(false);
            CouponsPanel.SetActive(false);

            CheckInternerConection();

            inputHandles.LoginClick += Authorization;
            inputHandles.RegistrationClick += Registration;
            inputHandles.RestorePasswordClick += ResetPassword;   

            FieldsCheker.ErrorSignEmail = EmailErrorSign;
            FieldsCheker.ErrorSignPassword = PasswordErrorSign;
            FieldsCheker.DescriptionText = TextUponFields;
        }

        void CheckInternerConection()
        {
            var webRequest = UnityWebRequest.Get("http://google.com");
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");

            StartCoroutine(Sender.SendWebRequest(webRequest, CheckInternerConectionResponse, ConectionError));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void Authorization()
        {
            if(!PlayerPrefs.HasKey("Token"))
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

            GetGold();
        }
        void Registration()
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

            RegistartionForm RegForm = new RegistartionForm(GlobalDataBase.Email, GlobalDataBase.Password);
            var webRequest = UnityWebRequest.Post(URLStruct.Registration, RegForm.Form);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            StartCoroutine(Sender.SendWebRequest(webRequest, RegistrationResponse, Errors));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void RegistrationResponse(string response)
        {
            Debug.Log(response);
        }

        void ResetPassword()
        {
            Debug.Log(GlobalDataBase.Email);
            var webRequest = UnityWebRequest.Post(URLStruct.ResetPassword + "?email=" + GlobalDataBase.Email, "");
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

        void GetGold()
        {
            var webRequest = UnityWebRequest.Get(URLStruct.GetCoin);
            webRequest.SetRequestHeader("Accept", "application/vnd.api+json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + GlobalDataBase.Token);
            StartCoroutine(Sender.SendWebRequest(webRequest, LoadLevel, Error));
            StartCoroutine(LoadIndicator.LoadAsynchronously(webRequest));
        }

        void LoadLevel(string response)
        {
            var Obj = JsonUtility.FromJson<Me>(response);
            GlobalDataBase.Gold = Convert.ToInt32(Obj.data.attributes.coin);

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

            Regex emailIsExist = new Regex(@"The email has already been taken.");
            Regex error500 = new Regex(@"Internal Server Error");
            Regex verifyEmail = new Regex(@"Your email address is not verified");
            Regex WrongFiledsData = new Regex(@"The provided authorization grant");
            Regex ResetPass = new Regex(@"Не удалось найти пользователя с указанным электронным адресом");

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

            if (WrongFiledsData.IsMatch(response))
            {
                EmailErrorSign.SetActive(true);
                PasswordErrorSign.SetActive(true);
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
            PanelWithText.transform.Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = "No internet Conection";
        }

        void CheckInternerConectionResponse(string response)
        {
            CheckForFirstStartup();
        }
    }
}