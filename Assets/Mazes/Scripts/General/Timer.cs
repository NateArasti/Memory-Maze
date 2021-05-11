using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Text time;
    [SerializeField] private float pathLengthScale = 1;

    public bool TimerStarted;
    private float timerCount;
    private float milliseconds;
    
    private void FixedUpdate()
    {
        if (TimerStarted && timerCount >= 0)
        {
            milliseconds -= (int)(100 * Time.fixedDeltaTime);
            if (milliseconds <= 0)
            {
                timerCount -= 1;
                if (timerCount < 0) TimerStarted = false;
                milliseconds = 100;
            }
            if(!TimerStarted) return;
            var ms = milliseconds < 10 ? $"0{milliseconds}" : $"{milliseconds}";
            time.text = timerCount < 10 ? $"0{timerCount}:{ms}" : $"{timerCount}:{ms}";
        }
    }

    public IEnumerator SetTimer(int pathLength)
    {
        timerCount = (int) (pathLength * pathLengthScale);
        for (var i = 0; i <= timerCount; i++)
        {
            yield return new WaitForSeconds(0.01f);
            time.text = i < 10 ? $"0{i}:00" : $"{i}:00";
        }
    }
}
