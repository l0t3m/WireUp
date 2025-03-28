using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "Scriptable Objects/LevelScriptableObject")]


public class LevelScriptableObject : ScriptableObject
{
    [Serializable]
    public struct rowData
    {
        public GridType[] types;
    }

    [SerializeField] public rowData[] LevelsMap = new rowData[4];
    [SerializeField] public int[] ItemLimits;
    [SerializeField] public int LevelNumber = 0;
    [SerializeField] public float timerLength = -1;
}
