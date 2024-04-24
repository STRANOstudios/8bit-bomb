using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameplaySettings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] TMP_Dropdown languageDropdown;

    void Start()
    {
        SetGameplay();
    }

    void OnEnable()
    {
        languageDropdown.onValueChanged.AddListener(delegate
        {
            SetLanguage(languageDropdown.value);
        });
    }

    void OnDisable()
    {
        languageDropdown.onValueChanged.RemoveListener(delegate
        {
            SetLanguage(languageDropdown.value);
        });
    }

    void SetLanguage(int value)
    {
        StartCoroutine(SetLocale(value));
        PlayerPrefs.SetInt("Language", value);
    }

    void SetGameplay()
    {
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(LocalizationSettings.AvailableLocales.Locales.Select(text => text.LocaleName).ToList());
        languageDropdown.value = GetSavedInt("Language");
        StartCoroutine(SetLocale(GetSavedInt("Language")));
        languageDropdown.RefreshShownValue();
    }

    IEnumerator SetLocale(int value)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
    }

    int GetSavedInt(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        return 0;
    }
}
