using UnityEngine;
using TMPro;

public class SetEmailToGlobalBase : MonoBehaviour
{
    [SerializeField] TMP_InputField EmailField = null;

    public void SetEmail()
    {
        GlobalDataBase.Email = EmailField.text;
    }
}
