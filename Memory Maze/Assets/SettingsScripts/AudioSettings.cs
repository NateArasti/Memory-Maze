using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private enum AudioType
    {
        Music,
        Sound
    }

#pragma warning disable 649
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource soundSource;
    private AudioSource musicSource;

    private void Start()
    {
        musicSource = MusicHandler.Instance.GetComponent<AudioSource>();
        SetUpAudio(AudioType.Sound, soundSlider, soundSource);
        SetUpAudio(AudioType.Music, musicSlider, musicSource);
    }

    private void SetUpAudio(AudioType audioType, Slider slider, AudioSource source)
    {
        var volume = PlayerPrefs.GetInt(audioType.ToString(), 100);
        slider.value = volume;
        source.volume = (float)volume / 100;
        slider.onValueChanged.AddListener(_ => ChangeVolume(audioType, slider.value));
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
}
