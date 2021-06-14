public class PerfectMazeGenerator : MazeGenerator
{
	public PerfectMazeGenerator() :
		base(MazeCharacteristics.Characteristics[MazeType.PerfectMaze].paramValues[0],
			MazeCharacteristics.Characteristics[MazeType.PerfectMaze].paramValues[1])
	{
	}

	protected override MazeCell FillTheMaze()
	{
		var cells = new PerfectMazeCell[Width, Height];
		for (var x = 0; x < Width; ++x)
		for (var y = 0; y < Height; ++y)
			cells[x, y] = new PerfectMazeCell();
		for (var x = 0; x < Width; ++x)
		for (var y = 0; y < Height; ++y)
		{
			if (x > 0)
				cells[x, y].Neighbors[(int) PerfectMazeCell.PossibleWalls.LeftWall] = cells[x - 1, y];
			if (y > 0)
				cells[x, y].Neighbors[(int) PerfectMazeCell.PossibleWalls.BottomWall] = cells[x, y - 1];
			if (x < Width - 1)
				cells[x, y].Neighbors[(int) PerfectMazeCell.PossibleWalls.RightWall] = cells[x + 1, y];
			if (y < Height - 1)
				cells[x, y].Neighbors[(int) PerfectMazeCell.PossibleWalls.UpperWall] = cells[x, y + 1];
			cells[x, y].SetPositions(x, y);
		}

		return cells[0, 0];
	}
}