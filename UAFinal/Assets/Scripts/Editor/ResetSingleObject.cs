using UnityEditor;
using UnityEngine;

public class ResetSingleObject
{
    [MenuItem("Tools/Reset 'Grid' Position")]
    public static void ResetPlayerPosition()
    {
        // Try to find a GameObject named "Player" in the scene
        GameObject Grid = GameObject.Find("Grid");

        if (Grid != null)
        {
            Undo.RecordObject(Grid.transform, "Reset Grid Position"); // Undo support
            Grid.transform.position = Vector3.zero;
            Debug.Log("Grid position reset to (0, 0, 0).");
        }
        else
        {
            Debug.LogWarning("No GameObject named 'Grid' found in the scene.");
        }
    }
}