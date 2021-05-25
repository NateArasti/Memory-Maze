using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
public abstract class MazeGenerator
{
    public readonly Maze Maze;

    public int Width { get; protected set; }
    public int Height { get; protected set; }

    protected MazeGenerator(int width, int height)
    {
        Width = width;
        Height = height;
        Maze = GenerateMaze();
    }

    protected abstract MazeCell FillTheMaze();

    private Maze GenerateMaze()
    {
        var startCell = FillTheMaze();

        var finishCell = RemoveWallsWithBacktracker(startCell);

        return new Maze(startCell, finishCell);
    }

    private MazeCell RemoveWallsWithBacktracker(MazeCell startCell)
    {
        var check = startCell.Visited;
        var furthest = startCell;
        var currentCell = startCell;
        currentCell.Visited = true;

        var cellStack = new Stack<MazeCell>();

        do
        {
            var unvisitedNeighbors = MazeCell.GetNeighborsList(currentCell, check);

            if (unvisitedNeighbors.Count > 0)
            {
                var chosenCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                RemoveWall(currentCell, chosenCell);

                chosenCell.Visited = !check;
                cellStack.Push(chosenCell);
                chosenCell.SetDistance(currentCell);
                if (chosenCell.DistanceFromStart > furthest.DistanceFromStart && chosenCell.Neighbors.Values.Contains(null))
                    furthest = chosenCell;
                currentCell = chosenCell;
            }
            else
            {
                currentCell = cellStack.Pop();
            }
        } while (cellStack.Count > 0);

        foreach (var neighbor in furthest.Neighbors)
        {
            if (neighbor.Value != null) continue;
            furthest.Walls[neighbor.Key] = false;
            break;
        }

        return furthest;
    }

    private void RemoveWall(MazeCell cellA, MazeCell cellB)
    {
        foreach (var cell in cellA.Neighbors)
            if (cellB == cell.Value)
            {
                cellA.Walls[cell.Key] = false;
                break;
            }
        foreach (var cell in cellB.Neighbors)
            if (cellA == cell.Value)
            {
                cellB.Walls[cell.Key] = false;
                break;
            }
    }
}