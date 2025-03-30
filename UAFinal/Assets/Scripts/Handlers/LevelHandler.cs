using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler Instance;
    [SerializeField] LevelScriptableObject[] levels;
    private LevelScriptableObject currentLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public LevelScriptableObject GetLevel()
    {
        return currentLevel;
    }

    public void StartGame()
    {
        currentLevel = levels[0];
    }

    public bool LevelComplete()
    {
        int levelNumber = currentLevel.LevelNumber;
        if (!IsLastLevel())
            currentLevel = levels[currentLevel.LevelNumber];
        levelNumber++;
        return levelNumber == levels.Length+1;
    }

    private bool IsLastLevel()
    {
        return currentLevel.LevelNumber == levels.Length;
    }

    // check if scene handler should return to menu
    public bool ShouldReturnToMenu()
    {
        return currentLevel.LevelNumber == levels.Length + 1;
    }

}
