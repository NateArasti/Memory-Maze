using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageSettings : MonoBehaviour
{
    private enum Languages
    {
        English,
        Russian
    }

#pragma warning disable 649
    [SerializeField] private Dropdown languageDropdown;

    private void Start()
    {
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Languages result);
        languageDropdown.value = resultBool ? (int)result : 0;
        languageDropdown.onValueChanged.AddListener(_ => ChangeLanguage(languageDropdown.value));
    }

    private void ChangeLanguage(int language)
    {
        PlayerPrefs.SetString("Language", ((Languages)language).ToString());
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }
}
