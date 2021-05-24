using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    [SerializeField] [TextArea] private string english;
    [SerializeField] [TextArea] private string russian;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
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