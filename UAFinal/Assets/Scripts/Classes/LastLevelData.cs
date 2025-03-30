using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int highestLevel;
    public SaveData(int highestLevel)
    {
        this.highestLevel = highestLevel;
    }

}