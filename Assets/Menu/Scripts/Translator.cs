using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    [SerializeField] private string english;
    [SerializeField] private string russian;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        if (!PlayerPrefs.HasKey("Language")) return;
        var lang = PlayerPrefs.GetString("Language");
        _text.text = lang switch
        {
            "English" => english,
            "Russian" => russian,
            _ => _text.text
        };
    }
}