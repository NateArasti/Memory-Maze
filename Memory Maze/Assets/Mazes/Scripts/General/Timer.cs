using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Text time;
    [SerializeField] [Range(0, 4)] private float pathLengthScale = 1;
    [SerializeField] private PlayerUI player;

    public bool TimerStarted { get; set; }
    public bool IsSpeedUp { get; set; }
    private float timerCount;
    private float milliseconds;
    
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
                player.Lose();
            }
            milliseconds = 99;
        }
        if(!TimerStarted) return;
        var ms = milliseconds < 10 ? $"0{milliseconds}" : $"{milliseconds}";
        time.text = timerCount < 10 ? $"0{timerCount}:{ms}" : $"{timerCount}:{ms}";
    }

    public IEnumerator SetTimer(int pathLength)
    {
        timerCount = (int) (pathLength * pathLengthScale);
        for (var i = 0; i <= timerCount; i++)
        {
            yield return new WaitForSeconds(0.05f);
            time.text = i < 10 ? $"0{i}:00" : $"{i}:00";
        }
    }

    public IEnumerator AddTime(int addTime)
    {
        var ms = milliseconds < 10 ? $"0{milliseconds}" : $"{milliseconds}";
        for (var i = 0; i <= (int)(addTime * pathLengthScale); i++)
        {
            TimerStarted = false;
            yield return new WaitForSeconds(0.05f);
            time.text = timerCount + i < 10 ? $"0{timerCount + i}:{ms}" : $"{timerCount + i}:{ms}";
        }
        timerCount += (int) (addTime * pathLengthScale);
        TimerStarted = true;
    }
}
