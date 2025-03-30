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
    [SerializeField] NavMeshSurface nms;
    LevelScriptableObject levelData;
    private NavMeshAgent powerObject;
    private Transform endObject;
    private Dictionary<BlockSection, int> itemLimits = new Dictionary<BlockSection, int>();

    private event Action OnGameStarted;

    private bool isFinished = false;
    private bool isStarted = false;
    private float idleTime = 5;
    [SerializeField] float idleTimeBase = 5;
    private Vector3 previousPosition;


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
        // check if power object is not null, if it isn't, check if it reached it's destination
        if (powerObject != null)
        {           
            if (Vector3.Distance(powerObject.transform.position, endObject.position) < 1.5f && !isFinished)
                Win();
            // if the power object is stationary for more than idleTimeBase, if so, lose the game
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
        }
    }

    // build the map from the levelData.LevelsMap
    private void BuildMap()
    {
        rowData[] mapPrimitive = levelData.LevelsMap;
        //loops through the 2d array
        for (int i = 0; i < mapPrimitive.Length; i++)
        {
            for (int j = 0; j < mapPrimitive[0].types.Length; j++)
            {
                // takes the block type that should be on the grid
                BlockSection currentSection = mapPrimitive[i].types[j];
                Vector3 newPos = new Vector3(levelData.TopLeftCorner.x + j * levelData.BlockDistance, levelData.TopLeftCorner.y, levelData.TopLeftCorner.z - i * levelData.BlockDistance);
                Snap currentGrid = Instantiate(blockData.GetGridObject(), newPos, new Quaternion()).GameObject().GetComponent<Snap>();
                currentGrid.OnSnap += HandleSnap;
                if (currentSection != BlockSection.Empty)
                {
                    newPos.y += 0.75f;
                    // creates the block type on the grid
                    DragAndDrop dragndrop = Instantiate(blockData.GetBlockByType(currentSection), newPos, new Quaternion()).GameObject().GetComponent<DragAndDrop>();
                    dragndrop.IsGenerated = true;
                    ExecuteNewPlaceableBlock(dragndrop);
                    newPos.y += 0.5f;
                    // if it's the start section, create the power object
                    if (currentSection == BlockSection.StartSection)
                    {
                        powerObject = Instantiate(powerPrefab, newPos, new Quaternion()).GameObject().GetComponent<NavMeshAgent>();
                        powerObject.isStopped = true;
                        powerObject.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                    }
                    // if its the end section, set the power objects goal
                    else if (currentSection == BlockSection.FinishSection)
                    {
                        newPos.y += 2f;
                        powerObject.SetDestination(newPos);
                        endObject = dragndrop.transform;
                    }
                }
            }
        }
    }

    // handle snap logic between grid and blocks
    private void HandleSnap(Snap grid, DragAndDrop block)
    {
        // if relation exists, dip
        if (relations.Find(relation => relation.IsPartOfCorrelation(grid, block)) != null) return;
        SnapCorrelation currentSnap = new SnapCorrelation(grid, block);
        relations.Add(currentSnap);

        // spawn the block's type on the right platform
        SpawnNewBlock(block);
        // updates all links in all relations because this game doesn't want to work without this
        foreach (var relation in relations)
        {
            foreach (var link in relation.block.GetComponentsInChildren<NavMeshLink>())
                link.UpdateLink();
        }
        
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
        powerObject.updateRotation = false;
        nms.BuildNavMesh();
        foreach (var relation in relations)
        {
            foreach (var link in relation.block.GetComponentsInChildren<NavMeshLink>())
                link.UpdateLink();
        }
        
        DetectDeadEnds();
    }

    private void DetectDeadEnds()
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 goalPos = endObject.position;

        foreach (Snap snap in FindObjectsByType<Snap>(FindObjectsSortMode.None))
        {
            SnapCorrelation relation = GetSnapCorrelation(snap);
            if (relation == null) continue;
            Vector3 start = relation.block.transform.position;
            if (NavMesh.SamplePosition(start, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                NavMesh.CalculatePath(hit.position, goalPos, NavMesh.AllAreas, path);

                if (path.status != NavMeshPathStatus.PathComplete)
                {
                    // Dead end found - handle it
                    Debug.Log($"Dead end at {relation.block.name}");                
                }
                else
                    powerObject.path = path;
            }
        }
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
        sceneHandler.LoadNextScene(LevelHandler.Instance.LevelComplete());
    }

    private void PlayRotateSound()
    {
        audioManager.PlaySound(AudioNames.Rotate);
    }

    private SnapCorrelation GetSnapCorrelation(Snap snap)
    {
        foreach (SnapCorrelation correlation in relations)
        {
            if (correlation.IsPartOfCorrelation(snap))
                return correlation;
        }
        return null;
    }

    private SnapCorrelation GetSnapCorrelation(DragAndDrop block)
    {
        foreach (SnapCorrelation correlation in relations)
        {
            if (correlation.IsPartOfCorrelation(block))
                return correlation;
        }
        return null;
    }
}
