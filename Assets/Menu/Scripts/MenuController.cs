using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Set in Inspector")] public GameObject MainMenu;
    public GameObject ChooseLanguagePanel;

    public void Awake()
    {
        if (MainMenu == null || ChooseLanguagePanel == null)
            return;
        if (!PlayerPrefs.HasKey("Language"))
            ChooseLanguagePanel.SetActive(true);
        else
            MainMenu.SetActive(true);
    }

    public void ChooseLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);
        GenerateScene("Menu");
    }

    public void GenerateScene(string sceneName)
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}