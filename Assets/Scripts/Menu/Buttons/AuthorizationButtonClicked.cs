using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AuthorizationButtonClicked : MonoBehaviour, IPointerClickHandler
{
    public static Action<byte> AccButtPres;

    //[SerializeField] GameObject EventSystem = null;

    #region type button
    private enum TypeOfButton
    {
        LogIn = 5,
        Registration = 4,
        AppleID = 3,
        Google = 1,
        Facebook = 2,
        VK = 0
    };

    [SerializeField] TypeOfButton TypeOfAutorisation = 0;
    #endregion

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        switch ((byte)TypeOfAutorisation)
        {
            #region case VK
            case 0:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
            #region case Google
            case 1:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
            #region case Facebook
            case 2:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
            #region case Apple 
            case 3:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
            #region case Usual
            case 4:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
            #region case Log In
            case 5:
                AccButtPres((byte)TypeOfAutorisation);

                break;
            #endregion
        }
    }
}
