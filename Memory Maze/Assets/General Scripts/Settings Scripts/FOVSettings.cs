using UnityEngine;
using UnityEngine.UI;

public class FOVSettings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Slider fovSlider;

    private void Start()
    {
        var fieldOfView = PlayerPrefs.GetInt("FieldOfView", 60);
        fovSlider.value = fieldOfView;
        MouseLook.FieldOfView = fieldOfView;
        fovSlider.onValueChanged.AddListener(_ =>
            ChangeMouseSensitivity((int)fovSlider.value));
    }

    private void ChangeMouseSensitivity(int fieldOfView)
    {
        MouseLook.FieldOfView = fieldOfView;
        PlayerPrefs.SetInt("FieldOfView", fieldOfView);
        PlayerPrefs.Save();
    }
}
