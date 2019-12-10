using UnityEngine;
using System.Collections.Generic;

public struct URLStruct
{
    public const string Registration = "http://wingift.cf/api/register";
    public const string Authorization = "http://wingift.cf/oauth/token";
    public const string SpeenWeel = "aoao";
    public const string SendCoins = "aoao";
}

public class RegistartionForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();

    public RegistartionForm(string email, string password)
    {
        Form.Add("email", email);
        Form.Add("password", password);
        Form.Add("password_confirmation", password);
    }
}

public class AuthorizationForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();

    public AuthorizationForm(string email, string password)
    {
        Form.Add("username", email);
        Form.Add("password", password);
        Form.Add("grant_type", "password");
        Form.Add("client_id", "2");
        Form.Add("client_secret", "kON2KfJmoCV6ve8tAQ4AL5BP917UaqxVzBZDZQqU");
    }
}

[System.Serializable]
public class ErrorResponse
{
    public List<ErrorData> errors;
}

[System.Serializable]
public class ErrorData
{
    public string status;
    public string title;
    public string detail;
}

[System.Serializable]
public class GetToken
{
    public string token_type;
    public string expires_in;
    public string access_token;
    public string refresh_token;
}




