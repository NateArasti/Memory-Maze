using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField]
    private string English = "null";
    [SerializeField]
    private string Russian = "null";

    private Text text;
    void Awake()
    {
        text = GetComponent<Text>();
        if (PlayerPrefs.HasKey("Language"))
        {
            if (PlayerPrefs.GetString("Language") == "English")
                text.text = English;
            else if (PlayerPrefs.GetString("Language") == "Russian")
                text.text = Russian;
        }
    }
}