using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Language
{
    English,
    Russian
}

public class LanguageSettings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private TMP_Dropdown languageDropdown;

    private void Start()
    {
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Language result);
        languageDropdown.value = resultBool ? (int)result : 0;
        languageDropdown.onValueChanged.AddListener(_ => ChangeLanguage(languageDropdown.value));
        LocalizationsFileParser.CreateDictionary();
    }

    private void ChangeLanguage(int language)
    {
        PlayerPrefs.SetString("Language", ((Language)language).ToString());
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
