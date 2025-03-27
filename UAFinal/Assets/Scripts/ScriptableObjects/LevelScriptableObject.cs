using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "Scriptable Objects/LevelScriptableObject")]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField] public Object[] LevelsMap;
    [SerializeField] public int[] ItemLimits;
}
