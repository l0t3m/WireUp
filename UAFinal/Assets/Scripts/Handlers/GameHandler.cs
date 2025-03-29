using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static LevelScriptableObject;

public class GameHandler : MonoBehaviour
{
    private List<SnapCorrelation> relations;

    [SerializeField] BlockData blockData;
    [SerializeField] LevelScriptableObject levelData;
    [SerializeField] InGameUIHandler gameUIHandler;  

    [SerializeField] Camera mainCamera;

    [SerializeField] Object powerPrefab;
    private NavMeshAgent powerObject;

    private Dictionary<BlockSection, int> itemLimits = new Dictionary<BlockSection, int>();

    

    private void Start()
    {
        // UI Related:
        gameUIHandler.TitleText.text = levelData.LevelNumber.ToString();
        gameUIHandler.timer.StartValue = levelData.TimerLength;

        // Correlations Related:
        relations = new List<SnapCorrelation>();


        BuildMap();

        // spawn default blocks
        for (int i = 0; i < levelData.GetLimitsLength(); i++)
        {
            BlockSection section = (BlockSection)(i+1);
            itemLimits.Add(section, levelData.GetItemLimit(section));
            SpawnNewBlock(section);
            gameUIHandler.UpdateBlocksLeftText(section, levelData.GetItemLimit(section));
        }
    }

    private void BuildMap()
    {
        rowData[] mapPrimitive = levelData.LevelsMap;      
        for (int i = 0; i < mapPrimitive.Length; i++)
        {
            for (int j = 0; j < mapPrimitive[0].types.Length; j++)
            {
                BlockSection currentSection = mapPrimitive[i].types[j];
                Vector3 newPos = new Vector3(levelData.TopLeftCorner.x + j * levelData.BlockDistance, levelData.TopLeftCorner.y, levelData.TopLeftCorner.z - i * levelData.BlockDistance);
                Snap currentGrid = Instantiate(blockData.GetGridObject(), newPos, new Quaternion()).GameObject().GetComponent<Snap>();
                currentGrid.OnSnap += HandleSnap;
                if (currentSection != BlockSection.Empty)
                {
                    newPos.y += 0.5f;
                    DragAndDrop dragndrop = Instantiate(blockData.GetBlockByType(currentSection), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>();
                    dragndrop.IsGenerated = true;
                    ExecuteNewPlaceableBlock(dragndrop);
                    if (currentSection == BlockSection.StartSection)
                    {
                        newPos.y += 0.5f;
                        powerObject = Instantiate(powerPrefab, newPos, new Quaternion()).GameObject().GetComponent<NavMeshAgent>();
                    }
                    else if (currentSection == BlockSection.FinishSection)
                    {
                        powerObject.SetDestination(newPos);
                        powerObject.isStopped = true;
                    }
                }
            }
        }
    }

    private void HandleSnap(Snap grid, DragAndDrop block)
    {
        if (relations.Find(relation => relation.IsPartOfCorrelation(grid, block)) != null) return;
        SnapCorrelation currentSnap = new SnapCorrelation(grid, block);
        relations.Add(currentSnap);

        SpawnNewBlock(block);

        currentSnap.ExecuteSnap();
    }

    private void SpawnNewBlock(DragAndDrop block)
    {
        if (!itemLimits.ContainsKey(block.BlockSection) || block.IsGenerated) return;
        itemLimits[block.BlockSection]--;
        gameUIHandler.UpdateBlocksLeftText(block.BlockSection, itemLimits[block.BlockSection]);
        if (itemLimits[block.BlockSection] > 0)
            ExecuteNewPlaceableBlock(Instantiate(block, block.OriginalPosition, new Quaternion()).GameObject().GetComponent<DragAndDrop>());

    }

    private void SpawnNewBlock(BlockSection section)
    {
        int newLimit = itemLimits[section];
        if (newLimit > 0)
        {
            Vector3 newPos = levelData.TopBlockPosition;
            newPos.z -= ((int)section-1) * levelData.BlockDistance;

            ExecuteNewPlaceableBlock(Instantiate(blockData.GetBlockByType(section), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>());
        }
    }

    private void ExecuteNewPlaceableBlock(DragAndDrop block)
    {
        block.currentCamera = mainCamera;
        block.OriginalPosition = block.transform.position;
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

    public void BeginAgentMovement()
    {
        powerObject.isStopped = false;
    }
}
