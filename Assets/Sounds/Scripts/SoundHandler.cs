using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    public Toggle toggle;
    public AudioSource audioSource;

    private void Start()
    {
        toggle.isOn = audioSource.volume == 1;
        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle.isOn);
        });
    }

    private void ToggleValueChanged(bool change)
    {
        if (audioSource != null)
            audioSource.volume = change ? 100 : 0;
    }
}
