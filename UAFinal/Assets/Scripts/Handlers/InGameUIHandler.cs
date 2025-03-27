using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] Timer timer;

    private void Start()
    {
        timer.onTimerEnd += ToggleLosePanel;
    }

    public void ToggleWinPanel()
    {
        WinPanel.gameObject.SetActive(true);
    }

    public void ToggleLosePanel()
    {
        LosePanel.gameObject.SetActive(true);
    }
}
