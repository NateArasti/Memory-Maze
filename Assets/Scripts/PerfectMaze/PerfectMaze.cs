using System.Collections.Generic;
using UnityEngine;

public class PerfectMazeGeneratorCell
{
    public int X;
    public int Y;

    public bool BottomWall = true;
    public bool LeftWall = true;
    public bool UpperWall = true;
    public bool RightWall = true;

    public bool Visited = false;
    public int DistanceFromStart;

    public Vector2Int prevDir = Vector2Int.zero;
    public PerfectMazeGeneratorCell prevNode;
    public bool isDeadEnd = false;
}

public class PerfectMaze
{
    public PerfectMazeGeneratorCell[,] cells;
    public PerfectMazeGeneratorCell finishPosition;
    public PerfectMazeGeneratorCell startPosition;
    public Dictionary<PerfectMazeGeneratorCell, Dictionary<PerfectMazeGeneratorCell, List<Vector2Int>>> nodes;
}
