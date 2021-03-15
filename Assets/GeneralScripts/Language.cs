using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField]
    private string English = "null";
    [SerializeField]
    private string Russian = "null";

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        if (!PlayerPrefs.HasKey("Language")) return;
        if (PlayerPrefs.GetString("Language") == "English")
            _text.text = English;
        else if (PlayerPrefs.GetString("Language") == "Russian")
            _text.text = Russian;
    }
}