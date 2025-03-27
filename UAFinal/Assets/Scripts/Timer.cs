using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public event Action onTimerEnd;

    [SerializeField] float StartValue = 60f;
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
            this.GetComponent<TextMeshProUGUI>().text = "Unlimited";
        }
        else if (timer > 0f)
        {
            timer -= Time.deltaTime;
            this.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00");
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
}
