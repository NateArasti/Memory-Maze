using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Set in Inspector")] public Sprite onSprite;
    public Sprite offSprite;
    public AudioSource audioSource;

    private Image _image;

    private void Start()
    {
        if (name == "Music")
            audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        _image = GetComponent<Image>();
    }

    public void ChangeSettings()
    {
        if (audioSource.volume == 0)
        {
            _image.sprite = onSprite;
            audioSource.volume = 100;
        }
        else
        {
            _image.sprite = offSprite;
            audioSource.volume = 0;
        }
    }
}