using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    [SerializeField] LevelScriptableObject[] Levels;
    [HideInInspector] public LevelScriptableObject CurrentLevel;

    void Awake()
    {
        this.InstantiateController();
        Time.timeScale = 1f;
    }

    private void InstantiateController()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        CurrentLevel = Levels[0]; // Fix
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

    public void LoadNextScene()
    {
        if (CurrentLevel == Levels[Levels.Length-1])
            LoadMainMenu();
        else
        {
            CurrentLevel = Levels[CurrentLevel.LevelNumber];
            ReloadScene();
        }
    }
}
