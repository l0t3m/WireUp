using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] GameObject OverUI;


    public void ToggleUI()
    {
        isPaused = !isPaused;
        OverUI.SetActive(isPaused);

        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
