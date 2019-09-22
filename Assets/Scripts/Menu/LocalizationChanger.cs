using UnityEngine;
using Assets.SimpleLocalization;

public class LocalizationChanger : MonoBehaviour
{
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
}
