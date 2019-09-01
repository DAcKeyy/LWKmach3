using System;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static Action LanguageSet = delegate { };

    private void SetLanguage(int Language)
    {
        PlayerPrefs.SetString("Language", Language.ToString());


        LanguageSet();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if(Application.systemLanguage == SystemLanguage.English)
            {
                PlayerPrefs.SetString("Language", 0.ToString());
            }
            else if(Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerPrefs.SetString("Language", 1.ToString());
            }
            else if(Application.systemLanguage == SystemLanguage.German)
            {
                PlayerPrefs.SetString("Language", 2.ToString());
            }
            else
            {
                PlayerPrefs.SetString("Language", 0.ToString());
            }

        }
        LanguageSet();
    }
}
