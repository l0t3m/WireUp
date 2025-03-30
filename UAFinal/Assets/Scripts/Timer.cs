using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public event Action onTimerEnd;

    public float StartValue = 60f;
    private float timer;
    private bool isFinished = false;

    private void Start()
    {
        timer = StartValue;
    }

    private void Update()
    {
        if (timer == -1)
        {
            this.GetComponent<TextMeshProUGUI>().text = FormatTimerText(timer);
            ChangeTextColor(Color.gray);
        }
        else if (timer > 0f)
        {
            if (timer <= 5f)
                ChangeTextColor(Color.red);
            this.GetComponent<TextMeshProUGUI>().text = FormatTimerText(timer);
            timer -= Time.deltaTime;
        }
        else
        {
            if (!isFinished)
            {
                onTimerEnd?.Invoke();
                isFinished = true;
            }
        }
    }

    private void ChangeTextColor(Color color)
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = color;
    }

    private string FormatTimerText(float value)
    {
        if (value == -1f)
            return "--:--";

        int minutes = Mathf.FloorToInt(value / 60f);
        int seconds = Mathf.FloorToInt(value % 60f);
        int hundredths = Mathf.FloorToInt((value * 100f) % 100f);

        if (minutes == 0)
            return string.Format("{0:00}:{1:00}", seconds, hundredths);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
    }
}
