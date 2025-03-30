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
            this.GetComponent<TextMeshProUGUI>().text = "--.--".Replace('.',':');
            ChangeTextColor(Color.gray);
        }
        else if (timer > 0f)
        {
            if (timer <= 5f)
                ChangeTextColor(Color.red);
            this.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00").Replace('.', ':');
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
}
