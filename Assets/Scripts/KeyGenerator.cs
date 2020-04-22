using System;
using System.Security.Cryptography;
using System.Text;

public class KeyGenerator
{
    SHA256Managed SHA = new SHA256Managed();

    public string GameKey()
    {
        string Key = DateTime.Now.ToString() + " UserID:" + GlobalDataBase.UserID + 
            " GameID:" + GlobalDataBase.MachThreeSessions;


        return Key;
    }

    public string SessionKey()
    {
        string Key = "";

        return Key;
    }
}
