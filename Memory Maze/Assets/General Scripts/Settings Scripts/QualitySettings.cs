using TMPro;
using UnityEngine;

public class QualitySettings : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("Quality", 1);
        UnityEngine.QualitySettings.SetQualityLevel(resolutionDropdown.value);
        resolutionDropdown.onValueChanged.AddListener(_ => ChangeResolution());
    }

    private void ChangeResolution()
    {
        PlayerPrefs.SetInt("Quality", resolutionDropdown.value);
        UnityEngine.QualitySettings.SetQualityLevel(resolutionDropdown.value);
        PlayerPrefs.Save();
    }
}
