using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public MazeGeneratorCell[,] cells;
    public MazeGeneratorCell finishPosition;
    public MazeGeneratorCell startPosition;
    public Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>> nodes;
}
