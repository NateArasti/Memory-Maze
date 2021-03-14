using System.Collections.Generic;
using UnityEngine;

public class PerfectMazeGenerator : MazeGenerator
{
    public PerfectMazeGenerator() :
        base(width: PlayerPrefs.HasKey("Width") ? PlayerPrefs.GetInt("Width") : 5,
        height: PlayerPrefs.HasKey("Height") ? PlayerPrefs.GetInt("Height") : 5)
    {
    }

    protected override void FillTheMaze()
    {
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                cells[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }
    }

    protected override void FillNeighboursList(
        List<MazeGeneratorCell> unvisitedNeighbours, 
        MazeGeneratorCell currentCell)
    {
        int x = currentCell.X;
        int y = currentCell.Y;

        if (x > 0 && !cells[x - 1, y].Visited)
            unvisitedNeighbours.Add(cells[x - 1, y]);
        if (y > 0 && !cells[x, y - 1].Visited)
            unvisitedNeighbours.Add(cells[x, y - 1]);
        if (x < width - 1 && !cells[x + 1, y].Visited)
            unvisitedNeighbours.Add(cells[x + 1, y]);
        if (y < height - 1 && !cells[x, y + 1].Visited)
            unvisitedNeighbours.Add(cells[x, y + 1]);
    }

    protected override MazeGeneratorCell MakeExit()
    {
        MazeGeneratorCell furthest = cells[0, 0];
        for (var x = 0; x < width; ++x)
        {
            if (cells[x, height - 1].DistanceFromStart > furthest.DistanceFromStart && cells[x, height - 1].isDeadEnd) 
                furthest = cells[x, height - 1];
            if (cells[x, 0].DistanceFromStart > furthest.DistanceFromStart && cells[x, 0].isDeadEnd)
                furthest = cells[x, 0];
        }
        for (var y = 0; y < height; ++y)
        {
            if (cells[width - 1, y].DistanceFromStart > furthest.DistanceFromStart && cells[width - 1, y].isDeadEnd)
                furthest = cells[width - 1, y];
            if (cells[0, y].DistanceFromStart > furthest.DistanceFromStart && cells[0, y].isDeadEnd)
                furthest = cells[0, y];
        }

        if (furthest.X == 0) furthest.LeftWall = false;
        else if (furthest.Y == 0) furthest.BottomWall = false;
        else if (furthest.X == width - 1) cells[furthest.X, furthest.Y].RightWall = false;
        else cells[furthest.X, furthest.Y].UpperWall = false;

        return new MazeGeneratorCell { X = furthest.X, Y = furthest.Y };
    }
}