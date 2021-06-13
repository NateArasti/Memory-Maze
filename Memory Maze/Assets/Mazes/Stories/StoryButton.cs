using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private int index;

    private void Awake()
    {
        var button = GetComponent<Button>();
        if (button == null) return;
        button.interactable = StoriesStorage.GetStoryByIndex(index, Language.English) != null;
    }

    public void ShowStory(TextMeshProUGUI storyContainer)
    {
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Language result);
        var currentLanguage = resultBool ? result : Language.English;
        var story = StoriesStorage.GetStoryByIndex(index, currentLanguage);
        if (story == null) return;
        storyContainer.text = story;
    }

    public void ExitStory(RectTransform scrollRectContent)
    {
        //Не знаю как тут нормально возвращать в начальное положение так что пусть будет так
        scrollRectContent.localPosition = new Vector3(0, -160, 0);
    }
}
