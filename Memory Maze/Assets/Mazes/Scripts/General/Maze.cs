public readonly struct Maze
{
    public readonly MazeCell StartCell;
    public readonly MazeCell FinishCell;
    //public readonly Dictionary<MazeCell, Dictionary<MazeCell, List<Vector2>>> Nodes;

    public Maze(MazeCell startCell, MazeCell finishCell)
    {
        StartCell = startCell;
        FinishCell = finishCell;
    }
}