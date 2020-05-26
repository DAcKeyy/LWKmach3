using System.Collections.Generic;

public struct URLStruct
{
    public const string Registration = "https://wingift.cf/api/register";
    public const string Authorization = "https://wingift.cf/api/login";
    public const string GetAccountInfo = "https://wingift.cf/api/v1/user/me";
    public const string DaylyCoupon = "https://wingift.cf/api/random";
    public const string ResetPassword = "https://wingift.cf/api/password/email";
    public const string GameSessionMessage = "https://wingift.cf/api/password/email";
    public const string LootBox = "https://wingift.cf/api/v1/user/lootbox";
    public const string Logout = "https://wingift.cf/api/logout";
    public static string CouponsLink;
    public static string GamesLink;
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
#region Forms
public class AuthorizationForm
{
    public Dictionary<string, string> Form = new Dictionary<string, string>();
    public AuthorizationForm(string email, string password)
    {
        Form.Add("email", email);
        Form.Add("password", password);
    }
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
#endregion
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
    public uint id;
    public Attributes attributes;
    public Relationships relationships;
    public LinksSelf links;
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
public class Relationships
{
    public Coupons coupons;
    public Games games;
}
[System.Serializable]
public class Coupons
{
    public LinksRelated links;
}
[System.Serializable]
public class Games
{
    public LinksRelated links;
}
[System.Serializable]
public class LinksSelf
{
    public string self;
}
[System.Serializable]
public class LinksRelated
{
    public string self;
    public string related;
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
#region LootBox
[System.Serializable]
public class LootBox
{
    public DataLootBox data;
}

[System.Serializable]
public class DataLootBox
{
    public string type;
    public uint id;
    public AttributesLootBox attributes;
    public LinksSelf links;
}
[System.Serializable]
public class AttributesLootBox
{
    public string buff_name;
    public string coupon;
    public string expiration_date;
    public string discount;
    public string company;
    public string description;
    public string contact;
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