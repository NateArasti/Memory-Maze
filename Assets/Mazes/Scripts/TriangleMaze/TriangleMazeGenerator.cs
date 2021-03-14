using System.Collections.Generic;
using UnityEngine;

public class TriangleMazeGenerator : MazeGenerator
{
    public TriangleMazeGenerator() :
        base(width: (PlayerPrefs.HasKey("Side") ? PlayerPrefs.GetInt("Side") : 5) * 2 - 1,
        height: PlayerPrefs.HasKey("Side") ? PlayerPrefs.GetInt("Side") : 5)
    {
    }

    protected override void FillTheMaze()
    {
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                if (y <= x && x < width - y)
                    cells[x, y] = new MazeGeneratorCell { X = x, Y = y };
                else
                    cells[x, y] = null;
            }
        }
    }

    protected override void FillNeighboursList(
        List<MazeGeneratorCell> unvisitedNeighbours,
        MazeGeneratorCell currentCell)
    {
        int x = currentCell.X;
        int y = currentCell.Y;

        if (x > 0 && cells[x - 1, y] != null && !cells[x - 1, y].Visited)
            unvisitedNeighbours.Add(cells[x - 1, y]);
        if (x < width - 1 && cells[x + 1, y] != null && !cells[x + 1, y].Visited)
            unvisitedNeighbours.Add(cells[x + 1, y]);
        if ((x + y) % 2 == 0 && y > 0 && cells[x, y - 1] != null && !cells[x, y - 1].Visited)
            unvisitedNeighbours.Add(cells[x, y - 1]);
        if ((x + y) % 2 == 1 && y < height - 1 && cells[x, y + 1] != null && !cells[x, y + 1].Visited)
            unvisitedNeighbours.Add(cells[x, y + 1]);
    }

    protected override MazeGeneratorCell MakeExit()
    {
        MazeGeneratorCell furthest = cells[0, 0];
        for (var y = 0; y < height; ++y)
        {
            if (cells[y, y].DistanceFromStart > furthest.DistanceFromStart && cells[y, y].isDeadEnd)
                furthest = cells[y, y];
            if (cells[width - y - 1, y].DistanceFromStart > furthest.DistanceFromStart && cells[width - y - 1, y].isDeadEnd)
                furthest = cells[width - y - 1, y];
        }
        for (var x = 0; x < width; x += 2) 
        {
            if (cells[x, 0].DistanceFromStart > furthest.DistanceFromStart && cells[x, 0].isDeadEnd)
                furthest = cells[x, 0];
        }

        if (furthest.X == furthest.Y) furthest.LeftWall = false;
        else if (furthest.Y == 0) furthest.BottomWall = false;
        else cells[furthest.X, furthest.Y].RightWall = false;

        return new MazeGeneratorCell { X = furthest.X, Y = furthest.Y };
    }
}