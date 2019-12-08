using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ButtonToShowPassword : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_InputField InputField = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(InputField.contentType == TMP_InputField.ContentType.Standard)
            InputField.contentType = TMP_InputField.ContentType.Password;
        else InputField.contentType = TMP_InputField.ContentType.Standard;

        InputField.enabled = false;
        InputField.enabled = true;
    }
}
