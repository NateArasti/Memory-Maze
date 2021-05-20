using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    private enum Languages
    {
        English,
        Russian
    }

    public enum AudioType
    {
        Music,
        Sound
    }

#pragma warning disable 649
    [SerializeField] private Dropdown languageDropdown;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private AudioSource soundSource;
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = MusicHandler.Instance.GetComponent<AudioSource>();

        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Languages result);
        languageDropdown.value = resultBool ? (int)result : 0;
        languageDropdown.onValueChanged.AddListener(_=>ChangeLanguage(languageDropdown.value));

        var soundVolume = PlayerPrefs.GetInt("Sound", 100);
        soundSlider.value = soundVolume;
        soundSource.volume = (float)soundVolume / 100;
        soundSlider.onValueChanged.AddListener(_=>ChangeVolume(AudioType.Sound, soundSlider.value));

        var musicVolume = PlayerPrefs.GetInt("Music", 100);
        musicSlider.value = musicVolume;
        musicSource.volume = (float)musicVolume / 100;
        musicSlider.onValueChanged.AddListener(_ => ChangeVolume(AudioType.Music, musicSlider.value));

        var mouseSensitivity = PlayerPrefs.GetInt("MouseSensitivity", 500);
        mouseSensitivitySlider.value = mouseSensitivity;
        mouseSensitivitySlider.onValueChanged.AddListener(_=> ChangeMouseSensitivity((int)mouseSensitivitySlider.value));
    }

    private void ChangeVolume(AudioType audioType, float newVolume)
    {
        PlayerPrefs.SetInt(audioType.ToString(), (int)newVolume);
        switch (audioType)
        {
            case AudioType.Music:
                musicSource.volume = newVolume / 100;
                break;
            case AudioType.Sound:
                soundSource.volume = newVolume / 100;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
        }
        PlayerPrefs.Save();
    }

    private void ChangeLanguage(int language)
    {
        PlayerPrefs.SetString("Language", ((Languages)language).ToString());
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }

    private void ChangeMouseSensitivity(int mouseSensitivity)
    {
        MouseLook.mouseSensitivity = mouseSensitivity;
        PlayerPrefs.SetInt("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }
}
