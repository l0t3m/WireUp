using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "Scriptable Objects/LevelScriptableObject")]


public class LevelScriptableObject : ScriptableObject
{
    [Serializable]
    public class rowData
    {
        public BlockSection[] types = new BlockSection[4];        
    }

    [SerializeField] public rowData[] LevelsMap = new rowData[4];
    [SerializeField] private int[] ItemLimits;
    [SerializeField] public int LevelNumber = 0;
    [SerializeField] public float TimerLength = -1;
    [SerializeField] public Vector3 TopBlockPosition;
    [SerializeField] public Vector3 TopLeftCorner;
    [SerializeField] public float BlockDistance = 10f;

    public int GetItemLimit(BlockSection section)
    {
        int sectionNum = (int)section - 1;
        return sectionNum >= ItemLimits.Length ? -1 : ItemLimits[sectionNum];
    }

    public int GetLimitsLength()
    {
        return ItemLimits.Length;
    }
}
