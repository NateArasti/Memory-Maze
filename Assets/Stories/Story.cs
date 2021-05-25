using System;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    public void ShowStory(int storyIndex)
    {
        var storyContainer = GetComponent<Text>();
        var resultBool = Enum.TryParse(PlayerPrefs.GetString("Language"), out Language result);
        var currentLanguage = resultBool ? result : Language.English;
        var story = StoriesStorage.GetStoryByIndex(storyIndex, currentLanguage);
        if (story == null) return;
        storyContainer.text = story;
    }

    public void ExitStory(RectTransform scrollRectContent)
    {
        //�� ���� ��� ��� ��������� ���������� � ��������� ��������� ��� ��� ����� ����� ���
        scrollRectContent.localPosition = new Vector3(0, -160, 0);
    }
}
