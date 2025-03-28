using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHandler : MonoBehaviour
{
    [SerializeField] DragAndDrop[] Blocks;
    [SerializeField] Snap[] Grid;
    private List<SnapCorrelation> relations;

    [SerializeField] LevelScriptableObject levelData;
    [SerializeField] InGameUIHandler gameUIHandler;

    private void Start()
    {
        // UI Related:
        gameUIHandler.TitleText.text = levelData.LevelNumber.ToString();
        gameUIHandler.timer.StartValue = levelData.timerLength;

        // Correlations Related:
        relations = new List<SnapCorrelation>();
        foreach (var obj in Grid )
        {
            obj.OnSnap += HandleSnap;
        }

        foreach (var obj in Blocks)
        {
            obj.OnUnsnap += HandleUnsnap;
            obj.MaxOfType = levelData.ItemLimits[(int)obj.BlockType];
        }
    }

    private void HandleSnap(Snap grid, DragAndDrop block)
    {
        if (relations.Find(relation => relation.IsPartOfCorrelation(grid, block)) != null) return;
        SnapCorrelation currentSnap = new SnapCorrelation(grid, block);
        relations.Add(currentSnap);
        currentSnap.ExecuteSnap();
    }

    private void HandleUnsnap(DragAndDrop block)
    {
        SnapCorrelation currentCurrelation = null;
        foreach (var Correlation in relations)
        {
            if (Correlation.IsPartOfCorrelation(block))
            {
                currentCurrelation = Correlation;
                break;
            }
        }

        if (currentCurrelation == null)
            return;

        currentCurrelation.ExecuteUnsnap();
        relations.Remove(currentCurrelation);
    }
}
