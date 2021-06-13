using TMPro;
using UnityEngine;

public class TextMeshLocalizer : MonoBehaviour
{
    private void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        var language = PlayerPrefs.GetString("Language");
        text.text = LocalizationsFileParser.Localizations[language][text.text];
    }
}
