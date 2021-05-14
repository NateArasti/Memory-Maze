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
        dropdown.onValueChanged.AddListener(_ => DropdownValueChanged(dropdown.value));
    }

    private static void DropdownValueChanged(int language)
    {
        PlayerPrefs.SetString("Language", ((Languages) language).ToString());
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }
}
