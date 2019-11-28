using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class JSON_Controller : MonoBehaviour
{
    #region Field
    public static Action<string> RegistrationErrors;
    public static Action<string> LoginErrors;
    public static Action<string> GetUserDataErrors;

    string URL;
    //[SerializeField] GameObject EventSystem = null;
    #endregion

    #region GET
    public void StartGET_JSON(int Type)
    {
        StartCoroutine(GET_JSON(Type));
    }


    public IEnumerator GET_JSON(int Type)
    { 
        if(Type == 0)
        {
            string URL = "https://wingift.cf/api/user/profile";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(URL))
            {
                webRequest.SetRequestHeader("Accept", "application/json");
                webRequest.SetRequestHeader("Authorization", GlobalDataBase.TypeOfToken + " " + GlobalDataBase.Token);

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    processJsonData(webRequest.downloadHandler.text, "GET", Type);
                }
            }
        }       
    }
    #endregion

    #region POST
    public void StartPOST_JSON(int Type)
    {
        StartCoroutine(POST_JSON(Type));
    }

    public IEnumerator POST_JSON(int Type)
    {
        #region Type 0: Register
        if (Type == 0)
        {
            URL = ("https://wingift.cf/api/register?email=" + GlobalDataBase.Email + "&name=" + GlobalDataBase.NickName +
                "&password=" + GlobalDataBase.Password + "&password_confirmation=" + GlobalDataBase.Password);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    processJsonData(webRequest.downloadHandler.text, "POST", Type);
                }
            }
        }
        #endregion
        #region Type 1: Logining
        if (Type == 1)
        {
            URL = ("https://wingift.cf/api/login?email=" + GlobalDataBase.Email + "&password=" + GlobalDataBase.Password);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, ""))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    processJsonData(webRequest.downloadHandler.text, "POST", Type);
                }
            }
        }
        #endregion

        if (Type == 2)
        {

        }
    }
    #endregion 

    private void processJsonData(string URL, string KindRequest, int Type)
    {
        if(KindRequest == "GET")
        {
            switch (Type)
            {
                case 0:
                    var CheckJSONGetUserData = JsonUtility.FromJson<user>(URL);

                    GlobalDataBase.NickName = CheckJSONGetUserData.name;
                    GlobalDataBase.IdInMedia = CheckJSONGetUserData.id;
                    //и ещё куча всего


                    GetUserDataErrors(CheckJSONGetUserData.error);

                    break;
                case 1:

                    break;
                case 2:

                    break;
            }
        }

        if(KindRequest == "POST")
        {
            switch (Type)
            {
                case 0:
                    var CheckJSONregister = JsonUtility.FromJson<register>(URL);                  

                    RegistrationErrors(CheckJSONregister.error);

                    break;
                case 1:
                    var CheckJSONlogin = JsonUtility.FromJson<login>(URL);

                    if (string.IsNullOrEmpty(CheckJSONlogin.error))
                    {
                        GlobalDataBase.Token = CheckJSONlogin.token;
                        GlobalDataBase.TypeOfToken = CheckJSONlogin.token_type;
                    }

                    Debug.Log(GlobalDataBase.Token);

                    
                    

                    LoginErrors(CheckJSONlogin.error);

                    break;
                case 2:

                    break;
            }
        }
    }
}
