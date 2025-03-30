using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        LevelHandler.Instance.StartGame();
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene(int levelNum)
    {
        if (LevelHandler.Instance.ShouldReturnToMenu())
            LoadMainMenu();
        else
            ReloadScene();
    }
}
