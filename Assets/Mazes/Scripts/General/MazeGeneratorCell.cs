using UnityEngine;

public class MazeGeneratorCell
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
    public MazeGeneratorCell prevNode;
    public bool isDeadEnd = false;
}
