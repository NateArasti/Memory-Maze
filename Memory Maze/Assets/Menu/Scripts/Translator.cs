using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] [TextArea] private string english;
    [SerializeField] [TextArea] private string russian;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (!PlayerPrefs.HasKey("Language")) return;
        var language = PlayerPrefs.GetString("Language");
        text.text = language switch
        {
            "English" => english,
            "Russian" => russian,
            _ => text.text
        };
    }
}