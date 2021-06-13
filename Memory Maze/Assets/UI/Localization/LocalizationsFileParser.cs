using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizationsFileParser : MonoBehaviour
{
	// Localizations["language"]["key"]
	public static Dictionary<string, Dictionary<string, string>> Localizations =
		new Dictionary<string, Dictionary<string, string>>();

	private static List<string> _languages = new List<string>();
	
	private static TextAsset _locales;
	
	public static void CreateDictionary()
	{
		Localizations = new Dictionary<string, Dictionary<string, string>>();
		_languages = new List<string>();
		_locales = Resources.Load<TextAsset>("Localizations");
		var languages = string.Join("", _locales.text.TakeWhile(x => x != '\n'))
			.Split(new[] {';', '\r'}, StringSplitOptions.RemoveEmptyEntries);
		foreach (var language in languages.Skip(1))
		{
			_languages.Add(language);
			Localizations.Add(language, new Dictionary<string, string>());
		}
		
		foreach (var keyValues in _locales.text.Split('\n').Skip(1))
		{
			var str = keyValues.Split(new[] {';', '\r'}, StringSplitOptions.RemoveEmptyEntries);
			for (var i = 1; i < str.Length; i++)
				Localizations[_languages[i - 1]].Add(str[0], str[i]);
		}
	}
}
