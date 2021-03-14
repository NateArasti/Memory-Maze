using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Text HighScore;
    public Text CurrentScore;
    public Text Grade;
    public GameObject NewHighScore;

    public void EndGame(int maxValue, int value, string sceneName)
    {
        var highscore = PlayerPrefs.HasKey(sceneName + "HighScore") ? PlayerPrefs.GetInt(sceneName + "HighScore") : 0;
        if (value > highscore)
        {
            NewHighScore.SetActive(true);
            PlayerPrefs.SetInt(sceneName + "HighScore", value);
            PlayerPrefs.Save();
            HighScore.text = value.ToString();
        }
        else
            HighScore.text = highscore.ToString();
        CurrentScore.text = value.ToString();
        var grade = Mathf.Round(100 * (float)value / maxValue);
        if (grade == 100 && maxValue > 1000)
            Grade.text = "SSS";
        else if (grade >= 90)
            Grade.text = "A+";
        else if (grade >= 80)
            Grade.text = "A";
        else if (grade >= 70)
            Grade.text = "B";
        else if (grade >= 60)
            Grade.text = "C+";
        else if (grade >= 40)
            Grade.text = "C";
        else if (grade >= 30)
            Grade.text = "D+";
        else if (grade >= 10)
            Grade.text = "D";
        else
            Grade.text = "F";
    }
}
