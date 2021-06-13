using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    public void ShowStory(int storyIndex)
    {
        var storyContainer = GetComponent<TextMeshProUGUI>();
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Language result);
        var currentLanguage = resultBool ? result : Language.English;
        var story = StoriesStorage.GetStoryByIndex(storyIndex, currentLanguage);
        if (story == null) return;
        storyContainer.text = story;
    }

    public void ExitStory(RectTransform scrollRectContent)
    {
        //Не знаю как тут нормально возвращать в начальное положение так что пусть будет так
        scrollRectContent.localPosition = new Vector3(0, -160, 0);
    }
}
