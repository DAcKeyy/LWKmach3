using System.Collections;
using UnityEngine;

public static class Prefs 
{
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
}
