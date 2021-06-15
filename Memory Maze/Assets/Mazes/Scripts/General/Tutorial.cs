using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private GameObject[] tutorialTexts;
	
	private int _currentIndex;

	private void Awake()
	{
		gameObject.SetActive(MazeLoader.IsTutorial);
	}

	public void SwitchToNextText()
	{
		tutorialTexts[_currentIndex].SetActive(false);
		_currentIndex += 1;
		tutorialTexts[_currentIndex].SetActive(true);
	}

	public void TurnOffAfterDelay(GameObject panel)
	{
		StartCoroutine(TurnOffWithDelay(panel));
	}

	private IEnumerator TurnOffWithDelay(GameObject panel)
	{
		yield return new WaitForSeconds(5f);
		panel.SetActive(false);
	}
}