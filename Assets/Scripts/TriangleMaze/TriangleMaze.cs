using System.Collections.Generic;
using UnityEngine;

public class TriangleMaze
{
    public TriangleMazeGeneratorCell[,] cells;
    public TriangleMazeGeneratorCell finishPosition;
    public TriangleMazeGeneratorCell startPosition;
    public Dictionary<TriangleMazeGeneratorCell, Dictionary<TriangleMazeGeneratorCell, List<Vector2Int>>> nodes;
}

public class TriangleMazeGeneratorCell
{
    public int X;
    public int Y;

    public bool BottomWall = true;
    public bool LeftWall = true;
    public bool RightWall = true;

    public bool Visited = false;
    public int DistanceFromStart;

    public Vector2Int prevDir = Vector2Int.zero;
    public TriangleMazeGeneratorCell prevNode;
    public bool isDeadEnd = false;
}
