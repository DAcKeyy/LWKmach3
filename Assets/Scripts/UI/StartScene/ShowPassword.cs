using UnityEngine;
using TMPro;
using LWT.System;
using Zenject;
public class ShowPassword : MonoBehaviour
{
    [SerializeField] 
    TMP_InputField InputField = null;
    [Inject]
    private StartSceneInputHandels inputHandles = null;

    public void Show()
    {
        if (InputField.contentType == TMP_InputField.ContentType.Standard)
            InputField.contentType = TMP_InputField.ContentType.Password;
        else InputField.contentType = TMP_InputField.ContentType.Standard;

        InputField.enabled = false;
        InputField.enabled = true;
    }

    private void Start()
    {
        inputHandles.ShowPasswordClick += Show;
    }
}
