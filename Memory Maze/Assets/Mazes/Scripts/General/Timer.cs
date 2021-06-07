using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private UnityEvent timerEnds;

    [Header("MazeModifiers")]
    [SerializeField] private float pathLengthScale = 1;
    [SerializeField] private float[] mazeTypeScales;
    [SerializeField] private float[] mazeDifficultyScales;
    [Header("TimerModifiers")]
    [SerializeField] private float maxAmountOfTimeToSetTimer = 5;
    [SerializeField] private float standardIntervalOfAdding = 0.05f;

    public bool TimerStarted { get; set; }
    public bool TimerSetted { get; private set; }
    public bool IsSpeedUp { get; set; }

    private int timerCount;
    private int milliseconds;
    
    private void FixedUpdate()
    {
        if (!TimerStarted || !(timerCount >= 0)) return;
        milliseconds -= (int)((IsSpeedUp ? 200 : 100) * Time.fixedDeltaTime);
        if (milliseconds <= 0)
        {
            timerCount -= 1;
            if (timerCount < 0)
            {
                TimerStarted = false;
                time.text = "00:00";
                timerEnds.Invoke();
            }
            milliseconds = 99;
        }
        if(!TimerStarted) return;
        var ms = milliseconds < 10 ? $"0{milliseconds}" : $"{milliseconds}";
        time.text = timerCount < 10 ? $"0{timerCount}:{ms}" : $"{timerCount}:{ms}";
    }

    public void SetTimer(int pathLength, Maze maze)
    {
        var newTimerCount = pathLength * pathLengthScale;

        newTimerCount *= mazeTypeScales[(int) MazeCharacteristics.CurrentMazeType];
        if (ArcadeProgression.ProgressionOn)
            newTimerCount *= mazeDifficultyScales[(int) ArcadeProgression.CurrentDifficulty];
        StartCoroutine(AddTime((int)newTimerCount));
    }

    public IEnumerator AddTime(int addTime)
    {
        TimerSetted = false;
        var interval = standardIntervalOfAdding;
        if (addTime * standardIntervalOfAdding > maxAmountOfTimeToSetTimer)
            interval = maxAmountOfTimeToSetTimer / addTime;
        var ms = milliseconds < 10 ? $"0{milliseconds}" : $"{milliseconds}";
        for (var i = 0; i <= addTime; i++)
        {
            TimerStarted = false;
            yield return new WaitForSeconds(interval);
            time.text = timerCount + i < 10 ? $"0{timerCount + i}:{ms}" : $"{timerCount + i}:{ms}";
        }
        timerCount += addTime;
        TimerSetted = true;
        TimerStarted = true;
    }
}
