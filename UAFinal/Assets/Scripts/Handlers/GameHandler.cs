using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static LevelScriptableObject;

public class GameHandler : MonoBehaviour
{
    [SerializeField] DragAndDrop[] Blocks;
    private List<SnapCorrelation> relations;

    [SerializeField] BlockData blockData;
    [SerializeField] LevelScriptableObject levelData;
    [SerializeField] InGameUIHandler gameUIHandler;

    [SerializeField] private Vector3 topLeftCorner;
    [SerializeField] private float blockDistance = 10f;

    [SerializeField] Camera mainCamera;

    private void Start()
    {
        // UI Related:
        gameUIHandler.TitleText.text = levelData.LevelNumber.ToString();
        gameUIHandler.timer.StartValue = levelData.timerLength;

        // Correlations Related:
        relations = new List<SnapCorrelation>();
        BuildMap();
    }

    private void BuildMap()
    {
        rowData[] mapPrimitive = levelData.LevelsMap;      
        for (int i = 0; i < mapPrimitive.Length; i++)
        {
            for (int j = 0; j < mapPrimitive[0].types.Length; j++)
            {
                BlockSection currentSection = mapPrimitive[i].types[j];
                Vector3 newPos = new Vector3(topLeftCorner.x + j * blockDistance, topLeftCorner.y, topLeftCorner.z - i * blockDistance);
                Snap currentGrid = Instantiate(blockData.GetGridObject(), newPos, new Quaternion()).GameObject().GetComponent<Snap>();
                currentGrid.OnSnap += HandleSnap;
                if (currentSection != BlockSection.Empty)
                {
                    newPos.y += 0.5f;
                    Instantiate(blockData.GetBlockByType(currentSection), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>().currentCamera = mainCamera;
                }
            }
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
