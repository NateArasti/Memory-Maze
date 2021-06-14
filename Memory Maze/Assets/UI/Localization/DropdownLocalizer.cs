using TMPro;
using UnityEngine;

public class DropdownLocalizer : MonoBehaviour
{
	private void Start()
	{
		var dropdown = GetComponent<TMP_Dropdown>();
		var language = PlayerPrefs.GetString("Language");
		foreach (var option in dropdown.options)
			option.text = LocalizationsFileParser.Localizations[language][option.text];

		var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
		if (LocalizationsFileParser.Localizations[language].ContainsKey(text.text))
			text.text = LocalizationsFileParser.Localizations[language][text.text];
	}
}