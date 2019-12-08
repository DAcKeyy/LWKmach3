using UnityEngine;
using Assets.SimpleLocalization;

public class LocalizationChanger : MonoBehaviour
{
    public void Awake()
    {
        LocalizationManager.Read();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.German:
                LocalizationManager.Language = "German";
                break;
            case SystemLanguage.Russian:
                LocalizationManager.Language = "Russian";
                break;
            default:
                LocalizationManager.Language = "English";
                break;
        }
    }

    public void SetLanguage(int Language)
    {
        switch (Language)
        {
            case 0:
                LocalizationManager.Language = "English";
                break;
            case 1:
                LocalizationManager.Language = "Russian";
                break;
            case 2:
                LocalizationManager.Language = "German";
                break;
        }
    }

    /// <summary>
    /// Change localization at runtime
    /// </summary>
    public void SetLocalization(string localization)
    {
        LocalizationManager.Language = localization;
    }
}
