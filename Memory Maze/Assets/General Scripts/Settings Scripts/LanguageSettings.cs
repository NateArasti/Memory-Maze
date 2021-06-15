using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Language
{
	English,
	Russian
}

public class LanguageSettings : MonoBehaviour
{
    public static string CurrentLanguage { get; private set; }

#pragma warning disable 649
	[SerializeField] private TMP_Dropdown languageDropdown;

	private void Awake()
	{
        CurrentLanguage = PlayerPrefs.GetString("Language", "English");
		var resultBool = Enum.TryParse(CurrentLanguage, out Language result);
		languageDropdown.value = resultBool ? (int) result : 0;
		languageDropdown.onValueChanged.AddListener(_ => ChangeLanguage(languageDropdown.value));
		LocalizationsFileParser.CreateDictionary();
	}

	private void ChangeLanguage(int language)
	{
		PlayerPrefs.SetString("Language", ((Language) language).ToString());
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}