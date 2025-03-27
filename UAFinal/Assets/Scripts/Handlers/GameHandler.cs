using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] List<DragAndDrop> Blocks;
    [SerializeField] List<Snap> Grid;
    private List<SnapCorrelation> relations;

    private void Start()
    {
        relations = new List<SnapCorrelation>();
        foreach (var obj in Grid )
        {
            obj.OnSnap += HandleSnap;
        }

        foreach (var obj in Blocks)
        {
            obj.OnUnsnap += HandleUnsnap;
        }
    }

    private void HandleSnap(Snap grid, DragAndDrop block)
    {
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
