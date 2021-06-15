using TMPro;
using UnityEngine;

public class FinishArcadeUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI mazeNumber;
	[SerializeField] private GameObject continueButton;
	[SerializeField] private GameObject arcadeEndText;

	private void Awake()
	{
		if (ArcadeProgression.CurrentIndex == ArcadeProgression.ProgressLevelsCount)
		{
			continueButton.SetActive(false);
			arcadeEndText.SetActive(true);
		}

		mazeNumber.text = $"{ArcadeProgression.CurrentIndex}";
	}
}