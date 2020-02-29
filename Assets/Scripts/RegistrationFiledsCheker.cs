using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class RegistrationFiledsCheker
{
    public GameObject ErrorSignEmail;
    public GameObject ErrorSignPassword;
    public TMP_Text DescriptionText;

    public bool CheckFields(TMP_InputField EmailField, TMP_InputField PasswordField)
    {
        if (!IsValidEmail(EmailField))
        {
            ErrorSignEmail.SetActive(true);
            DescriptionText.text = "Mail entered incorrectly!";
            return false;
        }

        if (!IsValidPassword(PasswordField))
        {
            ErrorSignPassword.SetActive(true);
            DescriptionText.text = "Password shouldn't be less then 8 letters!";
            return false;
        }
        return true;
    }

    private bool IsValidPassword(TMP_InputField Password) => Password.text.Length >= 8;
    private bool IsValidEmail(TMP_InputField Email) => Regex.IsMatch(Email.text, @"(@)(.+)$", RegexOptions.IgnoreCase);
}
