using UnityEngine;

public static class Prefs 
{
    public static int IsFirstTime
    {
        get { return PlayerPrefs.GetInt("IsFirstTime", 1); }
        set { PlayerPrefs.SetInt("IsFirstTime", value); }
    }

    public static int CoponsDeleted
    {
        get { return PlayerPrefs.GetInt("CoponsDeleted", 0); }
        set { PlayerPrefs.SetInt("CoponsDeleted", value); }
    }
    public static int CoponsAdded
    {
        get { return PlayerPrefs.GetInt("CoponsAdded", 0); }
        set { PlayerPrefs.SetInt("CoponsAdded", value); }
    }
    public static int CoponsAddedTotal
    {
        get { return PlayerPrefs.GetInt("CoponsAddedTotal", 0); }
        set { PlayerPrefs.SetInt("CoponsAddedTotal", value); }
    }
    public static int BombBuff
    {
        get { return PlayerPrefs.GetInt("BombBuff", 0); }
        set { PlayerPrefs.SetInt("BombBuff", value); }
    }
    public static int TimeBuff
    {
        get { return PlayerPrefs.GetInt("TimeBuff", 0); }
        set { PlayerPrefs.SetInt("TimeBuff", value); }
    }
}
