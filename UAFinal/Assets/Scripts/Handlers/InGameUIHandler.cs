using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] public Timer timer;
    [SerializeField] public TextMeshProUGUI TitleText;
    [SerializeField] public TextMeshProUGUI PauseCurrentLevelText;
    [SerializeField] public TextMeshProUGUI[] BlocksLeftText = new TextMeshProUGUI[4];

    private void Start()
    {
        PauseCurrentLevelText.text = $"Level {TitleText}";
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

    public void UpdateBlocksLeftText(BlockSection block, int newAmount)
    {
        switch (block)
        {
            case (BlockSection.StraightSection):
                BlocksLeftText[0].text = newAmount.ToString();
                break;
            case (BlockSection.LeftCornerSection):
                BlocksLeftText[1].text = newAmount.ToString();
                break;
            case (BlockSection.TSection):
                BlocksLeftText[2].text = newAmount.ToString();
                break;
            case (BlockSection.RightCornerSection):
                BlocksLeftText[3].text = newAmount.ToString();
                break;
        }
    }
}
