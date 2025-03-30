using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    public void SaveHighestLevel(int level)
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(new SaveData(level), true));
    }

    public int LoadHighestLevel()
    {
        return File.Exists(savePath) ? JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath)).highestLevel : 0;
    }
}
