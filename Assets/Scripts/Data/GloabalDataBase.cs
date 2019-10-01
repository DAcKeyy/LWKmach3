public static class GloabalDataBase
{
    public static int NumberOfWeel;
    public static float SoundValue;
    
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

