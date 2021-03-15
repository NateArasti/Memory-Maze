using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public MazeGeneratorCell[,] Cells;
    public MazeGeneratorCell FinishPosition;
    public MazeGeneratorCell StartPosition;
    public Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>> Nodes;
}