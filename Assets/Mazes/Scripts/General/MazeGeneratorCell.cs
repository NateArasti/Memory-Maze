using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool BottomWall = true;
    public bool LeftWall = true;
    public bool UpperWall = true;
    public bool RightWall = true;

    public bool Visited;
    public int DistanceFromStart;

    public Vector2Int PreviousDirection = Vector2Int.zero;
    public MazeGeneratorCell PreviousNode;
    public bool IsDeadEnd;
}
