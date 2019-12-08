public static class GlobalDataBase
{
    public static int NumberOfWeel;
    public static float SoundValue;
    public static float MusicValue;
    public static string Token = null;
    public static bool ConectionToServer = false;
    public static string FirstName = null;
    public static string SecondName = null;
    public static string Email = null;
    public static string NickName = null;
    public static string Password = null;
    public static string TypeOfToken = null;
    public static int Gold;
    public static int IdInMedia = 0;
}

[System.Serializable]
public class Сoupon
{
    public string company_name = "TWIRL Games";
    public string promo = "NONE";
    public string expiration_date = System.DateTime.Today.ToString("d");
    public string description = "Something goes wrong to this coupon";
    public string contact = "Contact \n twirlgamesteam@gmail.com";

    //public Сoupon(string Company, string Promo, string Expiration_date, string Description, string Contact)
    //{
    //    company_name = Company;
    //    promo = Promo;
    //    expiration_date = Expiration_date;
    //    description = Description;
    //    contact = Contact;
    //}
}


[System.Serializable]
public class user
{
    public int id;
    public string name;
    public string email;
    public string password;
    public string status;
    public string verify_token;
    public string created_at;
    public string updated_at;
    public string error;
}

[System.Serializable]
public class register
{
    public string message;
    public string verify;
    public string error;
}


[System.Serializable]
public class login
{
    public string token_type;
    public string token;
    public string expires_at;
    public string error;
}

