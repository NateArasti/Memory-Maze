using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionOption", 1);
        resolutionDropdown.onValueChanged.AddListener(_ =>
            ChangeResolution(resolutionDropdown.options[resolutionDropdown.value].text));

        fullScreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 0) == 0;
        fullScreenToggle.onValueChanged.AddListener(_ => ChangeFullscreen());
        Screen.fullScreen = fullScreenToggle.isOn;
    }

    private void ChangeResolution(string resolution)
    {
        PlayerPrefs.SetInt("ResolutionOption", resolutionDropdown.value);
        var res = resolution.Split('x');
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), fullScreenToggle.isOn);
        PlayerPrefs.Save();
    }

    private void ChangeFullscreen()
    {
        PlayerPrefs.SetInt("Fullscreen", fullScreenToggle.isOn ? 0 : 1);
        Screen.fullScreen = fullScreenToggle.isOn;
        PlayerPrefs.Save();
    }
}
