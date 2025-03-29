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

    public int LevelComplete()
    {
        int levelNumber = currentLevel.LevelNumber;
        if (!IsLastLevel())
            currentLevel = levels[currentLevel.LevelNumber];

        return currentLevel.LevelNumber;
    }

    public bool IsLastLevel()
    {
        return currentLevel.LevelNumber == levels.Length;
    }
}
