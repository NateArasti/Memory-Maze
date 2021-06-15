using TMPro;
using UnityEngine;

public class TextMeshLocalizer : MonoBehaviour
{
	private void Start()
	{
		var text = GetComponent<TextMeshProUGUI>();
        var language = LanguageSettings.CurrentLanguage;
		text.text = LocalizationsFileParser.GetTranslatedWordByKey(language, text.text);
	}
}