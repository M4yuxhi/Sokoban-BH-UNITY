using System.Collections.Generic;
using UnityEngine;

public class Level
{
    private List<Vector2> boxesInitialPos = new();
    private List<Vector2> floorGridsPos = new();
    private List<Vector2> goalsPos = new();
    private List<Vector2> wallsPos = new();

    private Vector2 playerInitialPos;

    public Level(List<Vector2> boxesInitialPos, List<Vector2> floorGridsPos, List<Vector2> goalsPos, List<Vector2> wallsPos, Vector2 playerInitialPos)
    {
        this.boxesInitialPos = boxesInitialPos;
        this.floorGridsPos = floorGridsPos;
        this.goalsPos = goalsPos;
        this.wallsPos = wallsPos;
        this.playerInitialPos = playerInitialPos;
    }

    public List<Vector2> BoxesInitialPos => boxesInitialPos;
    public List<Vector2> FloorGridsPos => floorGridsPos;
    public List<Vector2> GoalsPos => goalsPos;
    public List<Vector2> WallsPos => wallsPos;

    public Vector2 PlayerInitialPos => playerInitialPos;
}