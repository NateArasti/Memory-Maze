using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
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
    public bool TimerSet { get; private set; }
    public bool IsSpeedUp { get; set; }

    private int _timerCount;
    private int _milliseconds;
    
    private void FixedUpdate()
    {
        if (!TimerStarted || !(_timerCount >= 0)) return;
        _milliseconds -= (int)((IsSpeedUp ? 200 : 100) * Time.fixedDeltaTime);
        if (_milliseconds <= 0)
        {
            _timerCount -= 1;
            if (_timerCount < 0)
            {
                TimerStarted = false;
                time.text = "00:00";
                timerEnds.Invoke();
            }
            _milliseconds = 99;
        }
        if(!TimerStarted) return;
        var ms = _milliseconds < 10 ? $"0{_milliseconds}" : $"{_milliseconds}";
        time.text = _timerCount < 10 ? $"0{_timerCount}:{ms}" : $"{_timerCount}:{ms}";
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
        TimerSet = false;
        var interval = standardIntervalOfAdding;
        if (addTime * standardIntervalOfAdding > maxAmountOfTimeToSetTimer)
            interval = maxAmountOfTimeToSetTimer / addTime;
        var ms = _milliseconds < 10 ? $"0{_milliseconds}" : $"{_milliseconds}";
        for (var i = 0; i <= addTime; i++)
        {
            TimerStarted = false;
            yield return new WaitForSeconds(interval);
            time.text = _timerCount + i < 10 ? $"0{_timerCount + i}:{ms}" : $"{_timerCount + i}:{ms}";
        }
        _timerCount += addTime;
        TimerSet = true;
        TimerStarted = true;
    }
}
