using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static LevelScriptableObject;
using static Unity.Collections.AllocatorManager;

public class GameHandler : MonoBehaviour
{
    private List<SnapCorrelation> relations;

    [SerializeField] BlockData blockData;      
    [SerializeField] InGameUIHandler gameUIHandler;  
    [SerializeField] Camera mainCamera;
    [SerializeField] UnityEngine.Object powerPrefab;
    [SerializeField] SceneHandler sceneHandler;
    [SerializeField] AudioManager audioManager;
    LevelScriptableObject levelData;
    private NavMeshAgent powerObject;
    private Transform endObject;
    private Dictionary<BlockSection, int> itemLimits = new Dictionary<BlockSection, int>();

    private event Action OnGameStarted;

    private bool isFinished = false;
    private bool isStarted = false;
    private float idleTime = 5;
    [SerializeField] private const float idleTimeBase = 5;
    private Vector3 previousPosition;
    private GameObject previousNavMeshOwner;

    private void Start()
    {
        levelData = LevelHandler.Instance.GetLevel();

        // UI Related:
        gameUIHandler.TitleText.text = levelData.LevelNumber.ToString();
        gameUIHandler.timer.StartValue = levelData.TimerLength;
        gameUIHandler.timer.onTimerEnd += Lose;

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
        previousPosition = powerObject.transform.position;
    }

    private void Update()
    {
        if (powerObject != null)
        {           
            if (Vector3.Distance(powerObject.transform.position, endObject.position) < 1.5f && !isFinished)
                Win();
            else if (!isFinished && isStarted)
            {              
                if (Equals(previousPosition, powerObject.transform.position))
                {
                    idleTime -= Time.deltaTime;
                    if (idleTime <= 0f)
                        Lose();
                }
                else
                    idleTime = idleTimeBase;
            }
            previousPosition = powerObject.transform.position;
            
            if (powerObject.isOnNavMesh)
            {
                GameObject currentOwner = powerObject.navMeshOwner.GameObject();
                if (previousNavMeshOwner == currentOwner)
                {
                    previousNavMeshOwner = currentOwner;
                    currentOwner.GetComponent<DragAndDrop>().ChangeColor();

                }
            }
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
                    newPos.y += 0.75f;
                    DragAndDrop dragndrop = Instantiate(blockData.GetBlockByType(currentSection), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>();
                    dragndrop.IsGenerated = true;
                    ExecuteNewPlaceableBlock(dragndrop);
                    newPos.y += 0.5f;
                    if (currentSection == BlockSection.StartSection)
                    {
                        powerObject = Instantiate(powerPrefab, newPos, new Quaternion()).GameObject().GetComponent<NavMeshAgent>();
                        powerObject.isStopped = true;
                    }
                    else if (currentSection == BlockSection.FinishSection)
                    {
                        newPos.y += 2.5f;
                        powerObject.SetDestination(newPos);
                        endObject = dragndrop.transform;
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
        foreach (var link in block.GetComponentsInChildren<NavMeshLink>())       
            link.UpdateLink();
        
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
            DragAndDrop block = Instantiate(blockData.GetBlockByType(section), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>();
            block.OnRotate += PlayRotateSound;
            
            ExecuteNewPlaceableBlock(block);
        }
    }

    private void ExecuteNewPlaceableBlock(DragAndDrop block)
    {
        block.currentCamera = mainCamera;
        block.OriginalPosition = block.transform.position;
        OnGameStarted += block.DisableActions;
        
    }

    public void BeginAgentMovement()
    {
        OnGameStarted?.Invoke();
        isStarted = true;
        powerObject.isStopped = false;
    }

    private void Win()
    {
        isFinished = true;
        powerObject.isStopped = true;
        gameUIHandler.ToggleWinPanel();
        audioManager.PlaySound(AudioNames.Win);
    }

    private void Lose()
    {
        isFinished = true;
        powerObject.isStopped = true;
        gameUIHandler.ToggleLosePanel();
        audioManager.PlaySound(AudioNames.Lose);
    }

    public void NextButtonPressed()
    {
        LevelHandler.Instance.LevelComplete();
        sceneHandler.LoadNextScene();
    }

    private void PlayRotateSound()
    {
        audioManager.PlaySound(AudioNames.Rotate);
    }
}
