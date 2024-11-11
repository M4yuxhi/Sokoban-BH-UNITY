using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Offset Fixes")]
    [SerializeField] private float offsetX = -7.5f;
    [SerializeField] private float offsetY = -6f;

    [Header("Prefabs")]
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall;

    [Header("Level")]
    [SerializeField] private int levelIndex;

    [HideInInspector] public LevelNode currentNode;

    public static LevelManager instance;

    private List<Transform> boxesTrans;

    private Transform playerTrans;

    void OnEnable() => LevelNode.UpdateScreen += UpdatePositions;    
    
    void OnDisable() => LevelNode.UpdateScreen -= UpdatePositions;

    void Start()
    {
        if (instance == null) instance = this;

        boxesTrans = new List<Transform>();

        StartCoroutine(WaitForLevels());
    }
    public void UpdatePositions(LevelNode node)
    {
        if (!(LevelReader.Levels != null && LevelReader.LevelCount > 0)) return;

        StartCoroutine(ParallelUpdatePositions(node));
    }

    private IEnumerator ParallelUpdatePositions(LevelNode node)
    {
        Vector2 playerPos = new();
        List<Vector2> boxesPos = new();
        Level level = LevelReader.Levels[levelIndex];
        int boxesAmount = level.BoxesInitialPos.Count;
        int boxesCount = 0;
        bool playerFound = false;
        bool shouldStopOuterLoop = false;

        for (int i = 0; i < level.Size.x; i++)
        {
            for (int j = 0; j < level.Size.y; j++)
            {
                if (playerFound && boxesCount >= boxesAmount)
                {
                    shouldStopOuterLoop = true;
                    break;
                }

                short element = node.matrixValues[i, j];

                if (element == 2) // Player found
                {
                    playerFound = true;
                    playerPos = new Vector2(i, j);
                }
                else if (element == 3) // Box found
                {
                    boxesCount++;
                    boxesPos.Add(new Vector2(i, j));
                }
            }

            if (shouldStopOuterLoop) break;
        }

        playerTrans.position = playerPos;

        for (int i = 0; i < boxesCount; i++)
            boxesTrans[i].position = boxesPos[i];

        yield return null; // Optionally, yield to split the execution across frames
    }


    private IEnumerator WaitForLevels()
    {
        yield return new WaitUntil(() => LevelReader.Levels != null && LevelReader.LevelCount > 0);
    
        InstantiateLevelElements();
    }

    private void InstantiateLevelElements()
    {
        Level level = LevelReader.Levels[levelIndex];

        foreach (var box in level.BoxesInitialPos)
        {
            GameObject boxi = Instantiate(this.box, new Vector2(box.x + offsetX, box.y + offsetY), Quaternion.Euler(0, 0, 0));
            boxesTrans.Add(boxi.transform);
        }
        foreach (var grid in level.FloorGridsPos)
            Instantiate(floor, new Vector2(grid.x + offsetX, grid.y + offsetY), Quaternion.Euler(0, 0, 0));

        foreach (var goal in level.GoalsPos)
            Instantiate(this.goal, new Vector2(goal.x + offsetX, goal.y + offsetY), Quaternion.Euler(0, 0, 0));

        foreach (var wall in level.WallsPos)
            Instantiate(this.wall, new Vector2(wall.x + offsetX, wall.y + offsetY), Quaternion.Euler(0, 0, 0));

        Vector2 playerPos = level.PlayerInitialPos;
        GameObject pluyer = Instantiate(player, new Vector2(playerPos.x + offsetX, playerPos.y + offsetY), Quaternion.Euler(0, 0, 0));
        playerTrans = pluyer.transform;
    }
}