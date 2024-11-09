using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Offset Fixes")]
    [SerializeField] private float offsetX = -6.5f;
    [SerializeField] private float offsetY = 5f;

    [Header("Prefabs")]
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall;

    [Header("Level")]
    [SerializeField] private int levelIndex;

    void Start() => StartCoroutine(WaitForLevels());

    private IEnumerator WaitForLevels()
    {
        yield return new WaitUntil(() => LevelReader.Levels != null && LevelReader.LevelCount > 0);
    
        InstantiateLevelElements();
    }

    private void InstantiateLevelElements()
    {
        Level level = LevelReader.Levels[levelIndex];

        foreach (var box in level.BoxesInitialPos)
            Instantiate(this.box, new Vector2(box.x + offsetX, box.y + offsetY), Quaternion.Euler(0, 0, 0));

        foreach (var grid in level.FloorGridsPos)
            Instantiate(floor, new Vector2(grid.x + offsetX, grid.y + offsetY), Quaternion.Euler(0, 0, 0));

        foreach (var goal in level.GoalsPos)
            Instantiate(this.goal, new Vector2(goal.x + offsetX, goal.y + offsetY), Quaternion.Euler(0, 0, 0));

        foreach (var wall in level.WallsPos)
            Instantiate(this.wall, new Vector2(wall.x + offsetX, wall.y + offsetY), Quaternion.Euler(0, 0, 0));

        Vector2 playerPos = level.PlayerInitialPos;
        Instantiate(player, new Vector2(playerPos.x + offsetX, playerPos.y + offsetY), Quaternion.Euler(0, 0, 0));
    }
}