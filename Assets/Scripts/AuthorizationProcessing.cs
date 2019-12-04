using UnityEngine;
using TMPro;
using System;

public class AuthorizationProcessing : MonoBehaviour
{
    [SerializeField] GameObject PasswordErrorSign = null;
    [SerializeField] GameObject EmailErrorSign = null;
    [SerializeField] TMP_InputField EmailField = null;
    [SerializeField] TMP_InputField PasswordField = null;
    [SerializeField] TMP_Text TextUponFields = null;
    [SerializeField] LevelLoader levelLoader = null;

    WebSender Sender = new WebSender();
    RegistrationFiledsCheker FieldsCheker = new RegistrationFiledsCheker();

    public static Action FirstStartUp;

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
        FieldsCheker.ErrorSignEmail = EmailErrorSign;
        FieldsCheker.ErrorSignPassword = PasswordErrorSign;
        FieldsCheker.DescriptionText = TextUponFields;

        if (PlayerPrefs.HasKey("Token"))
        {
            //StartCoroutine(Sender.POST(URLStruct.Authorization,  ,TokenAuthorization));
            //AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);
            AuthorizationForm AuthForm = new AuthorizationForm(GlobalDataBase.Email, GlobalDataBase.Password);

            StartCoroutine(Sender.POST(URLStruct.Authorization, AuthForm.Form, Authorization));
        }

        else 
        {
            FirstStartUp();
        }
    }

    void StartRqesting (byte type)
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

            StartCoroutine(Sender.POST(URLStruct.Authorization, AuthForm.Form, Authorization));
        }

        if(type == 4) //Registration
        {
            Debug.Log(GlobalDataBase.Email);

            RegistartionForm RegForm = new RegistartionForm(GlobalDataBase.Email, GlobalDataBase.Password);

            StartCoroutine(Sender.POST(URLStruct.Registration, RegForm.Form, Registration));
        }
    }



    void Authorization(string Response)
    {
        Debug.Log(Response);


        /*else
         * {
         *     levelLoader.LoadLevel("Menu");
         * }
         * 
         */
    }

    void Registration(string Response)
    {
        Debug.Log(Response);
    }

    void TokenAuthorization(string Response)
    {
        Debug.Log(Response);
    }
}