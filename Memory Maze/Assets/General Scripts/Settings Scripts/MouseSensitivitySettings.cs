using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySettings : MonoBehaviour
{
#pragma warning disable 649
	[SerializeField] private Slider mouseSensitivitySlider;

	private void Start()
	{
		var mouseSensitivity = PlayerPrefs.GetInt("MouseSensitivity", 500);
		mouseSensitivitySlider.value = mouseSensitivity;
		MouseLook.MouseSensitivity = mouseSensitivity;
		mouseSensitivitySlider.onValueChanged.AddListener(_ =>
			ChangeMouseSensitivity((int) mouseSensitivitySlider.value));
	}

	private void ChangeMouseSensitivity(int mouseSensitivity)
	{
		MouseLook.MouseSensitivity = mouseSensitivity;
		PlayerPrefs.SetInt("MouseSensitivity", mouseSensitivity);
		PlayerPrefs.Save();
	}
}