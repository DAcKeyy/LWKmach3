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
    public static uint UserID = 0;
    public static uint MachThreeSessions;
    public static int Tickets = 0;
    public static string PrevScene = "Start";

    public static class ChestJSONs
    {
        public static System.Collections.Generic.List<string> JSONs;

        static ChestJSONs()
        {
            JSONs = new System.Collections.Generic.List<string>();
        }

        public static void Add(string value)
        {
            JSONs.Add(value);
        }
    }

}

[System.Serializable]
public class Сoupon
{
    public string company_name = "TWIRL Games";
    public string promo = "NONE";
    public string expiration_date = System.DateTime.Today.ToString("d");
    public string description = "Something goes wrong to this coupon";
    public string contact = "Contact \n twirlgamesteam@gmail.com";
}

