using UnityEngine;
using UnityEngine.UI;

public class FinishArcadeUI : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Text mazeNumber;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject arcadeEndText;

    private void Awake()
    {
        if (ArcadeProgression.CurrentIndex == ArcadeProgression.ProgressLevelsCount)
        {
            continueButton.SetActive(false);
            arcadeEndText.SetActive(true);
        }
        mazeNumber.text = $"{ArcadeProgression.CurrentIndex} {mazeNumber.text.Split(' ')[1]}";
    }
}
