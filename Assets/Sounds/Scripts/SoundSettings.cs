using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Sprite onSprite;
    public Sprite offSprite;
    public AudioSource audioSource;

    private Image image;

    private void Start()
    {
        if (name == "Music")
            audioSource = GameObject.Find("Background Music").GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }

    public void ChangeSettings()
    {
        if(audioSource.volume == 0)
        {
            image.sprite = onSprite;
            audioSource.volume = 100;
        }
        else
        {
            image.sprite = offSprite;
            audioSource.volume = 0;
        }
    }
}
