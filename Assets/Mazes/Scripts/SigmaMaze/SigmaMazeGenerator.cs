using UnityEngine;

public class SigmaMazeGenerator : MazeGenerator
{
    private const int StandardSideLength = 3;

    public SigmaMazeGenerator() : 
        base((PlayerPrefs.HasKey("SigmaSide") ? PlayerPrefs.GetInt("SigmaSide") : StandardSideLength) * 2 - 1,
            (PlayerPrefs.HasKey("SigmaSide") ? PlayerPrefs.GetInt("SigmaSide") : StandardSideLength) * 2 - 1)
    {
    }

    protected override MazeCell FillTheMaze()
    {
        const int bottomRight = (int)SigmaMazeCell.PossibleWalls.BottomRightWall;
        const int bottomLeft = (int)SigmaMazeCell.PossibleWalls.BottomLeftWall;
        const int upperRight = (int)SigmaMazeCell.PossibleWalls.UpperRightWall;
        const int upperLeft = (int)SigmaMazeCell.PossibleWalls.UpperLeftWall;
        const int left = (int)SigmaMazeCell.PossibleWalls.LeftWall;
        const int right = (int)SigmaMazeCell.PossibleWalls.RightWall;

        var cells = new SigmaMazeCell[Height][];
        var side = (Width + 1) / 2;
        for (var i = 0; i < side; i++)
        {
            cells[i] = new SigmaMazeCell[side + i];
            FillArray(cells[i]);
        }
        for (var i = side; i < Height; i++)
        {
            cells[i] = new SigmaMazeCell[Width + side - i - 1];
            FillArray(cells[i]);
        }
        for (var y = 0; y < Height; y++)
        {
            var delta = Width - cells[y].Length;
            for (var x = 0; x < cells[y].Length; x++, delta++)
            {
                if (x > 0)
                    cells[y][x].Neighbors[left] = cells[y][x - 1];

                if (x < cells[y].Length - 1)
                    cells[y][x].Neighbors[right] = cells[y][x + 1];

                if (y > side - 1)
                {
                    cells[y][x].Neighbors[bottomLeft] = cells[y - 1][x];
                    cells[y][x].Neighbors[bottomRight] = cells[y - 1][x + 1];
                    if (y < Height - 1)
                    {
                        if (x < cells[y + 1].Length)
                            cells[y][x].Neighbors[upperRight] = cells[y + 1][x];
                        if (x > 0)
                            cells[y][x].Neighbors[upperLeft] = cells[y + 1][x - 1];
                    }
                }
                else if (y < side - 1) 
                {
                    cells[y][x].Neighbors[upperLeft] = cells[y + 1][x];
                    cells[y][x].Neighbors[upperRight] = cells[y + 1][x + 1];
                    if (y > 0)
                    {
                        if (x < cells[y - 1].Length)
                            cells[y][x].Neighbors[bottomRight] = cells[y - 1][x];
                        if (x > 0)
                            cells[y][x].Neighbors[bottomLeft] = cells[y - 1][x - 1];
                    }
                }
                else
                {
                    if (x < cells[y + 1].Length)
                        cells[y][x].Neighbors[upperRight] = cells[y + 1][x];
                    if (x > 0)
                        cells[y][x].Neighbors[upperLeft] = cells[y + 1][x - 1];
                    if (x < cells[y - 1].Length)
                        cells[y][x].Neighbors[bottomRight] = cells[y - 1][x];
                    if (x > 0)
                        cells[y][x].Neighbors[bottomLeft] = cells[y - 1][x - 1];
                }
                
                cells[y][x].SetPositions(x + delta, y);
            }
        }
        
        return cells[0][0];
    }

    private void FillArray(SigmaMazeCell[] cellArray)
    {
        for (var j = 0; j < cellArray.Length; j++)
        {
            cellArray[j] = new SigmaMazeCell();
        }
    }
}
