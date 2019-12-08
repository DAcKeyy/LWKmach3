using System;
using System.Collections.Generic;



// Account created
[Serializable]
public class RegisterGet
{
    public string status;
    public account Account;    
}

[Serializable]
public class account
{
    public string access_token;
    public int expires_in;
    public int user_id;
}
//


