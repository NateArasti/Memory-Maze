using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] tutorialTexts;
#pragma warning restore 649
    private int currentIndex;

    private void Awake()
    {
        gameObject.SetActive(MazeLoader.IsTutorial);
    }

    public void SwitchToNextText()
    {
        tutorialTexts[currentIndex].SetActive(false);
        currentIndex += 1;
        tutorialTexts[currentIndex].SetActive(true);
    }

    public void TurnOffAfterDelay(GameObject panel) => StartCoroutine(TurnOffWithDelay(panel));

    private IEnumerator TurnOffWithDelay(GameObject panel)
    {
        yield return new WaitForSeconds(5f);
        panel.SetActive(false);
    }
}
