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

    //private Dictionary<MazeCell, Dictionary<MazeCell, List<Vector2>>> GetAllNodes(MazeCell startCell)
    //{
    //    var nodes = new Dictionary<MazeCell, Dictionary<MazeCell, List<Vector2>>>();
    //    var stackOfNodes = new Stack<MazeCell>();
    //    maze.StartCell.PreviousDirection = Vector2Int.zero;
    //    stackOfNodes.Push(maze.StartCell);
    //    var unvisitedDirections = new Stack<MazeGeneratorCell>();
    //    do
    //    {
    //        var currentNode = stackOfNodes.Pop();
    //        unvisitedDirections.Clear();
    //        var nextNodes = new Dictionary<MazeGeneratorCell, List<Vector2Int>>();
    //        unvisitedDirections = FindPossibleDirections(currentNode);
    //        while (unvisitedDirections.Count > 0)
    //        {
    //            var currentCell = unvisitedDirections.Pop();
    //            var count = 0;
    //            var path = new List<Vector2Int> { currentCell.PreviousDirection };
    //            while (!CheckIfNode(currentCell))
    //            {
    //                currentCell = GetNextCell(currentCell, path);
    //            todo: Убрать костыль!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //            count++;
    //                if (count > Width * Height) break;
    //            }

    //            stackOfNodes.Push(currentCell);
    //            nextNodes[currentCell] = path;
    //            currentCell.PreviousNode = currentNode;
    //        }

    //        nodes[currentNode] = nextNodes;
    //        if (nextNodes.Count == 0)
    //            currentNode.IsDeadEnd = true;
    //    } while (stackOfNodes.Count > 0);

    //    return nodes;
    //}

    //private bool CheckIfNode(MazeCell currentCell)
    //{
    //    var count = 0;

    //    if (!currentCell.LeftWall) count++;
    //    if (!currentCell.BottomWall) count++;
    //    if (!currentCell.RightWall) count++;
    //    if (!currentCell.UpperWall) count++;

    //    return count != 2;
    //}

    //private Stack<MazeCell> FindPossibleDirections(MazeCell currentNode)
    //{
    //    var dirs = new Stack<MazeCell>();
    //    var x = currentNode.X;
    //    var y = currentNode.Y;

    //    if (!cells[x, y].LeftWall && x > 0 && cells[x - 1, y] != null &&
    //        currentNode.PreviousDirection != Vector2Int.right)
    //    {
    //        cells[x - 1, y].PreviousDirection = Vector2Int.left;
    //        dirs.Push(cells[x - 1, y]);
    //    }

    //    if (!cells[x, y].BottomWall && y > 0 && currentNode.PreviousDirection != Vector2Int.up)
    //    {
    //        cells[x, y - 1].PreviousDirection = Vector2Int.down;
    //        dirs.Push(cells[x, y - 1]);
    //    }

    //    if (!cells[x, y].RightWall && x < Width - 1 && cells[x + 1, y] != null &&
    //        currentNode.PreviousDirection != Vector2Int.left)
    //    {
    //        cells[x + 1, y].PreviousDirection = Vector2Int.right;
    //        dirs.Push(cells[x + 1, y]);
    //    }

    //    if (!cells[x, y].UpperWall && currentNode.Y < Height - 1 && currentNode.PreviousDirection != Vector2Int.down)
    //    {
    //        cells[x, y + 1].PreviousDirection = Vector2Int.up;
    //        dirs.Push(cells[x, y + 1]);
    //    }

    //    return dirs;
    //}

    //private MazeCell GetNextCell(MazeCell currentCell, List<Vector2Int> path)
    //{
    //    var x = currentCell.X;
    //    var y = currentCell.Y;

    //    if (!cells[x, y].LeftWall &&
    //        x > 0 && cells[x - 1, y] != null &&
    //        currentCell.PreviousDirection != Vector2Int.right)
    //    {
    //        path.Add(Vector2Int.left);
    //        currentCell = cells[x - 1, y];
    //        currentCell.PreviousDirection = Vector2Int.left;
    //    }
    //    else if (!cells[x, y].BottomWall &&
    //             y > 0 &&
    //             currentCell.PreviousDirection != Vector2Int.up)
    //    {
    //        path.Add(Vector2Int.down);
    //        currentCell = cells[x, y - 1];
    //        currentCell.PreviousDirection = Vector2Int.down;
    //    }
    //    else if (!cells[x, y].RightWall &&
    //             x < Width - 1 && cells[x + 1, y] != null &&
    //             currentCell.PreviousDirection != Vector2Int.left)
    //    {
    //        path.Add(Vector2Int.right);
    //        currentCell = cells[x + 1, y];
    //        currentCell.PreviousDirection = Vector2Int.right;
    //    }
    //    else if (!cells[x, y].UpperWall &&
    //             currentCell.Y < Height - 1 &&
    //             currentCell.PreviousDirection != Vector2Int.down)
    //    {
    //        path.Add(Vector2Int.up);
    //        currentCell = cells[x, y + 1];
    //        currentCell.PreviousDirection = Vector2Int.up;
    //    }

    //    return currentCell;
    //}
}