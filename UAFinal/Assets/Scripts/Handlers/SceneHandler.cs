using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] GameObject NextLevel = null;

    private void Start()
    {
        Time.timeScale = 1; // Due to the UI Handler freezing timescale
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        if (NextLevel != null)
        {
            SceneManager.LoadScene(NextLevel.name);
        }
    }
}
