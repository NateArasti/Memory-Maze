using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizationsFileParser
{
	// Localizations["language"]["key"]
	private static Dictionary<string, Dictionary<string, string>> _localizations;
	private static List<string> _languages;
	private static TextAsset _locales;

    public static string GetTranslatedWordByKey(string language, string key) =>
        _localizations.ContainsKey(language) && _localizations[language].ContainsKey(key) ? 
            _localizations[language][key] : key;

    public static void CreateDictionary()
	{
		_localizations = new Dictionary<string, Dictionary<string, string>>();
		_languages = new List<string>();
		_locales = Resources.Load<TextAsset>("Localizations");
		var languages = string.Join("", _locales.text.TakeWhile(x => x != '\n'))
			.Split(new[] {';', '\r'}, StringSplitOptions.RemoveEmptyEntries);
		foreach (var language in languages.Skip(1))
		{
			_languages.Add(language);
			_localizations.Add(language, new Dictionary<string, string>());
		}

		foreach (var keyValues in _locales.text.Split('\n').Skip(1))
		{
			var str = keyValues.Split(new[] {';', '\r'}, StringSplitOptions.RemoveEmptyEntries);
			for (var i = 1; i < str.Length; i++)
				_localizations[_languages[i - 1]].Add(str[0], str[i]);
		}
	}
}