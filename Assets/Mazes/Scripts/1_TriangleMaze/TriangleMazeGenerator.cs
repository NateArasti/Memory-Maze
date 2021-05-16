public class TriangleMazeGenerator : MazeGenerator
{
    private const int StandardSideLength = 5;

    public TriangleMazeGenerator() :
        base(MazeCharacteristics.Characteristics[MazeType.TriangleMaze].paramValues[0] * 2 - 1,
            MazeCharacteristics.Characteristics[MazeType.PerfectMaze].paramValues[0])
    {
    }

    protected override MazeCell FillTheMaze()
    {
        var bottom = (int)TriangleMazeCell.PossibleWalls.BottomWall;
        var left = (int)TriangleMazeCell.PossibleWalls.LeftWall;
        var right = (int)TriangleMazeCell.PossibleWalls.RightWall;

        var cells = new TriangleMazeCell[Height][];
        for (var y = 0; y < Height; y++)
        {
            cells[Height - 1 - y] = new TriangleMazeCell[2 * y + 1];
            for (var x = 0; x < cells[Height - 1 - y].Length; x++)
            {
                cells[Height - 1 - y][x] = new TriangleMazeCell();
            }
        }
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < cells[y].Length; x++)
            {
                if (x % 2 == 1) cells[y][x].IsUpsideDown = true;
                if (x > 0)
                    cells[y][x].Neighbors[left] = cells[y][x - 1];
                if (y > 0 && !cells[y][x].IsUpsideDown)
                    cells[y][x].Neighbors[bottom] = cells[y - 1][x + 1];
                if (y < Height - 1 && cells[y][x].IsUpsideDown)
                    cells[y][x].Neighbors[bottom] = cells[y + 1][x - 1];
                if (x < cells[y].Length - 1)
                    cells[y][x].Neighbors[right] = cells[y][x + 1];
                if(!cells[y][x].IsUpsideDown) cells[y][x].SetPositions(x + y, y);
            }
        }

        return cells[0][0];
    }
}