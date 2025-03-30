
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    public event Action OnSceneLeft;
    public SaveHandler saveHandler;
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        OnSceneLeft?.Invoke();
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        int level = saveHandler.LoadHighestLevel();
        LevelHandler.Instance.LoadLevel(level == LevelHandler.Instance.GetLevelsLength() ? 0 : level);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene(bool returnToMenu)
    {
        if (returnToMenu)
            LoadMainMenu();
        else
            ReloadScene();
    }
}
