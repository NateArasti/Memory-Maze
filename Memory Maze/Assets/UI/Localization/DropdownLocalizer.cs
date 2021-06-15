using TMPro;
using UnityEngine;

public class DropdownLocalizer : MonoBehaviour
{
	private void Start()
	{
		var dropdown = GetComponent<TMP_Dropdown>();
		var language = LanguageSettings.CurrentLanguage;
        foreach (var option in dropdown.options)
            option.text = LocalizationsFileParser.GetTranslatedWordByKey(language, option.text);
        var newText = LocalizationsFileParser.GetTranslatedWordByKey(language, dropdown.captionText.text);
        dropdown.captionText.text = newText;
    }
}