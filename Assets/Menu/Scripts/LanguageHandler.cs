using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageHandler : MonoBehaviour
{
    public enum Languages
    {
        English, 
        Russian
    }
    
    public Dropdown dropdown;

    private void Start()
    {
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Languages result);
        dropdown.value = resultBool ? (int)result : 0;
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown.value); });
    }

    public void DropdownValueChanged(int language)
    {
        switch (language)
        {
            case 0:
                PlayerPrefs.SetString("Language", "English");
                break;
            case 1:
                PlayerPrefs.SetString("Language", "Russian");
                break;
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }
}
