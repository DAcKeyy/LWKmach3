using UnityEngine;
using System;

public class PlayerPreffsManager
{

    #region For Sound
    public float GetSoundValue()
    {
        if (PlayerPrefs.HasKey("SoundValue"))
        {
            return Convert.ToSingle(PlayerPrefs.GetString("SoundValue"));
        }
        else
        {
            PlayerPrefs.SetString("SoundValue", "1");
            return 1;
        }
    }

    public void SetSoundValue(float value)
    {
        if (PlayerPrefs.HasKey("SoundValue"))
        {
            PlayerPrefs.SetString("SoundValue", value.ToString());
        }
        else
        {
            PlayerPrefs.SetString("SoundValue", "1");
        }
    }
    #endregion
    #region For Music
    public float GetMusicValue()
    {
        if (PlayerPrefs.HasKey("MusicValue"))
        {
            return Convert.ToSingle(PlayerPrefs.GetString("MusicValue"));
        }
        else
        {
            PlayerPrefs.SetString("MusicValue", "0");
            return 0;
        }
    }

    public void SetMusicValue(float value)
    {
        if(PlayerPrefs.HasKey("MusicValue"))
        {
            PlayerPrefs.SetString("MusicValue", value.ToString());
        }
        else
        {
            PlayerPrefs.SetString("MusicValue", "0");
        }
    }
    #endregion
}
