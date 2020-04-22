using UnityEngine;
using System.Collections.Generic;


public struct URLStruct
{
    public const string Registration = "https://wingift.cf/api/register";
    public const string Authorization = "https://wingift.cf/oauth/token";
    public const string SpinWeel = "https://wingift.cf/api/coupons/roulette";
    public const string SendCoins = "https://wingift.cf/api/users/costs";
    public const string GetCoin = "https://wingift.cf/api/users/me";
    public const string DaylyCoupon = "https://wingift.cf/api/random";
    public const string ResetPassword = "https://wingift.cf/api/password/email";
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

#region SendJsonGame
[System.Serializable]
public class GameSessionMessage
{
    public GameSessionData data;
}

[System.Serializable]
public class GameSessionData
{
    public string type = "games";
    public AttributesGameSession attributes;
}

[System.Serializable]
public class AttributesGameSession
{
    public uint game_id = 1;
    public uint user_id = 1;
    public string session_date = "null";
}
#endregion

public class AuthorizationForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();

    public AuthorizationForm(string email, string password)
    {
        Form.Add("username", email);
        Form.Add("password", password);
        Form.Add("grant_type", "password");
        Form.Add("client_id", "2");
        Form.Add("client_secret", "c6BkvrlXqYbIT9eGzXoJhfHWZFfijRPSZXZ0yG66");
    }
}

public class TokenForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();

    public TokenForm(string Token)
    {
        Form.Add("Bearer Token", Token);
    }
}

public class ToIncreaseForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();

    public ToIncreaseForm(string value)
    {
        Form.Add("increase", value);
    }
}

[System.Serializable]
public class NetworkError
{
    public string Error;
}

#region Me
[System.Serializable]
public class Me
{
    public Data data;
}

[System.Serializable]
public class Data
{
    public string type;
    public string id;
    public Attributes attributes;
    public Links links;
}

[System.Serializable]
public class Attributes
{
    public string email;
    public string email_verified_at;
    public string coin;
    public string created_at;
    public string updated_at;
}

[System.Serializable]
public class Links
{
    public string self;
}
#endregion
#region Roulette/Coins
[System.Serializable]
public class RoulleteResponse
{
    public metaData meta;
}

[System.Serializable]
public class metaData
{
    public string type;
    public string title;
    public string replacment;
}
#endregion 
#region Roulette/Coupon
[System.Serializable]
public class CouponResponse
{
    public CouponData data;
}

[System.Serializable]
public class CouponData
{
    public string type;
    public uint id;
    public CouponAttributes attributes;
}

[System.Serializable]
public class CouponAttributes
{
    public string coupon;
    public string expiration_date;
    public string discount;
    public string company;
    public string description;
    public string contact;
}
#endregion
#region Error
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
#endregion

[System.Serializable]
public class GetToken
{
    public string token_type;
    public string expires_in;
    public string access_token;
    public string refresh_token;
}