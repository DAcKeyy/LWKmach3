using System;
using UnityEngine;
using TMPro;

public class TextTransletor : MonoBehaviour
{
    private string language;
    private TMP_Text Text;

    [SerializeField] string EnglishText;
    [SerializeField] string RussianText;
    [SerializeField] string GermanText;

    private void OnEnable()
    {
        LocalizationManager.LanguageSet += Set;
    }

    private void OnDisable()
    {
        LocalizationManager.LanguageSet -= Set;
    }

    private void Set()
    {
        Text = this.GetComponent<TMP_Text>();

        language = PlayerPrefs.GetString("Language");

        switch (Int32.Parse(language))
        {
            case 0:
                Text.text = EnglishText;

                break;
            case 1:
                Text.text = RussianText;

                break;
            case 2:
                Text.text = GermanText;

                break;

            default:
                Text.text = EnglishText;
                break;
        }
    }
}
