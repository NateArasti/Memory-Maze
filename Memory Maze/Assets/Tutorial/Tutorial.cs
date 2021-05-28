using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Sprite[] tutorialSlides;
    [SerializeField] private GameObject[] descriptions;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;

    private int index;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        index = 0;
        UpdateSlide();
        CheckSlide();
    }

    private void UpdateSlide()
    {
        image.sprite = tutorialSlides[index];
        descriptions[index].SetActive(true);
    }

    private void CheckSlide()
    {
        previousButton.SetActive(index != 0);
        nextButton.SetActive(index != tutorialSlides.Length - 1);
    }

    public void Next()
    {
        descriptions[index].SetActive(false);
        index += 1;
        UpdateSlide();
        CheckSlide();
    }

    public void Previous()
    {
        descriptions[index].SetActive(false);
        index -= 1;
        UpdateSlide();
        CheckSlide();
    }

    public void Exit()
    {
        index = 0;
        UpdateSlide();
        CheckSlide();
    }
}
