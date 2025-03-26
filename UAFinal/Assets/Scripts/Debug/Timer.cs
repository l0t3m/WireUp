using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer = 0.0f;


    private void Update()
    {
        timer += Time.deltaTime;
        this.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00");
    }
}
