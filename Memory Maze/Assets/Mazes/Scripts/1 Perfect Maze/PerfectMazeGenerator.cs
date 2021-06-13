public class PerfectMazeGenerator : MazeGenerator
{
    public PerfectMazeGenerator() :
        base(MazeCharacteristics.Characteristics[MazeType.PerfectMaze].paramValues[0],
            MazeCharacteristics.Characteristics[MazeType.PerfectMaze].paramValues[1])
    {
    }

    protected override MazeCell FillTheMaze()
    {
        var bottom = (int)PerfectMazeCell.PossibleWalls.BottomWall;
        var left = (int)PerfectMazeCell.PossibleWalls.LeftWall;
        var upper = (int)PerfectMazeCell.PossibleWalls.UpperWall;
        var right = (int)PerfectMazeCell.PossibleWalls.RightWall;
        var cells = new PerfectMazeCell[Width, Height];
        for (var x = 0; x < Width; ++x)
        for (var y = 0; y < Height; ++y)
            cells[x, y] = new PerfectMazeCell();
        for (var x = 0; x < Width; ++x)
            for (var y = 0; y < Height; ++y)
            {
                if (x > 0)
                    cells[x,y].Neighbors[left] = cells[x - 1, y];
                if (y > 0)
                    cells[x, y].Neighbors[bottom] = cells[x, y - 1];
                if (x < Width - 1)
                    cells[x, y].Neighbors[right] = cells[x + 1, y];
                if (y < Height - 1)
                    cells[x, y].Neighbors[upper] = cells[x, y + 1];
                cells[x,y].SetPositions(x, y);
            }

        return cells[0, 0];
    }
}