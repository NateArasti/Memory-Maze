using System.Collections.Generic;
using UnityEngine;

public class PerfectMazeGenerator
{
    public int width = PlayerPrefs.HasKey("Width") ? PlayerPrefs.GetInt("Width") : 5;
    public int height = PlayerPrefs.HasKey("Height") ? PlayerPrefs.GetInt("Height") : 5;

    public PerfectMaze GenerateMaze()
    {
        var cells = new PerfectMazeGeneratorCell[width, height];

        for(var x = 0; x < width; ++x)
        {
            for(var y = 0; y < height; ++y)
            {
                cells[x, y] = new PerfectMazeGeneratorCell { X = x, Y = y };
            }
        }

        RemoveWallsWithBackracker(cells);

        var maze = new PerfectMaze();
        maze.cells = cells;
        maze.startPosition = new PerfectMazeGeneratorCell { X = 0, Y = 0 };
        maze.nodes = GetAllNodes(maze);
        maze.finishPosition = MakeExit(cells);

        return maze;
    }

    private void RemoveWallsWithBackracker(PerfectMazeGeneratorCell[,] cells)
    {
        PerfectMazeGeneratorCell currentCell = cells[0,0];
        currentCell.Visited = true;
        currentCell.DistanceFromStart = 0;

        var cellStack = new Stack<PerfectMazeGeneratorCell>();

        do
        {
            var unvisitedNeighbours = new List<PerfectMazeGeneratorCell>();

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

            if (unvisitedNeighbours.Count > 0)
            {
                PerfectMazeGeneratorCell chosenCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(currentCell, chosenCell);

                chosenCell.Visited = true;
                cellStack.Push(chosenCell);
                chosenCell.DistanceFromStart = currentCell.DistanceFromStart + 1;
                currentCell = chosenCell;
            }
            else
                currentCell = cellStack.Pop();

        } while(cellStack.Count > 0);
    }

    private void RemoveWall(PerfectMazeGeneratorCell cellA, PerfectMazeGeneratorCell cellB)
    {
        if(cellA.X == cellB.X)
        {
            if(cellA.Y > cellB.Y)
            {
                cellA.BottomWall = false;
                cellB.UpperWall = false;
            }
            else
            {
                cellA.UpperWall = false;
                cellB.BottomWall = false;
            }
        }
        else
        {
            if (cellA.X > cellB.X)
            {
                cellA.LeftWall = false;
                cellB.RightWall = false;
            }
            else
            {
                cellA.RightWall = false;
                cellB.LeftWall = false;
            }
        }
    }

    private PerfectMazeGeneratorCell MakeExit(PerfectMazeGeneratorCell[,] cells)
    {
        PerfectMazeGeneratorCell furthest = cells[0, 0];
        for (var x = 0; x < cells.GetLength(0); ++x)
        {
            if (cells[x, height - 1].DistanceFromStart > furthest.DistanceFromStart && cells[x, height - 1].isDeadEnd) 
                furthest = cells[x, height - 1];
            if (cells[x, 0].DistanceFromStart > furthest.DistanceFromStart && cells[x, 0].isDeadEnd)
                furthest = cells[x, 0];
        }
        for (var y = 0; y < cells.GetLength(1); ++y)
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

        return new PerfectMazeGeneratorCell { X = furthest.X, Y = furthest.Y };
    }

    //Возвращает узлы(развилки или тупики) лабиринта
    private Dictionary<PerfectMazeGeneratorCell, Dictionary<PerfectMazeGeneratorCell, List<Vector2Int>>> GetAllNodes(PerfectMaze maze)
    {
        var nodes = new Dictionary<PerfectMazeGeneratorCell, Dictionary<PerfectMazeGeneratorCell, List<Vector2Int>>>();
        var cells = maze.cells;
        var stackOfNodes = new Stack<PerfectMazeGeneratorCell>();
        maze.startPosition.prevDir = Vector2Int.zero;
        stackOfNodes.Push(maze.startPosition);
        var unvistedDirections = new Stack<PerfectMazeGeneratorCell>();
        do
        {
            var currentNode = stackOfNodes.Pop();
            unvistedDirections.Clear();
            var nextNodes = new Dictionary<PerfectMazeGeneratorCell, List<Vector2Int>>();

            unvistedDirections = FindPossibleDirections(cells, currentNode);

            while(unvistedDirections.Count > 0)
            {
                var currentCell = unvistedDirections.Pop();
                int count = 0;
                var path = new List<Vector2Int>();
                path.Add(currentCell.prevDir);
                while (!CheckIfNode(currentCell))
                {
                    currentCell = GetNextCell(cells, currentCell, path);
                    //Убрать костыль!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    count++;
                    if (count > width * height) break;
                }
                stackOfNodes.Push(currentCell);
                nextNodes[currentCell] = path;
                currentCell.prevNode = currentNode;
            }
            nodes[currentNode] = nextNodes;
            if (nextNodes.Count == 0)
                currentNode.isDeadEnd = true;
        } while (stackOfNodes.Count > 0);

        return nodes;
    }

    private bool CheckIfNode(PerfectMazeGeneratorCell currentCell)
    {
        int count = 0;

        if (!currentCell.LeftWall) count++;
        if (!currentCell.BottomWall) count++;
        if (!currentCell.RightWall) count++;
        if (!currentCell.UpperWall) count++;

        return count != 2 ? true : false;
    }

    private Stack<PerfectMazeGeneratorCell> FindPossibleDirections(PerfectMazeGeneratorCell[,] cells, PerfectMazeGeneratorCell currentNode)
    {
        var dirs = new Stack<PerfectMazeGeneratorCell>();
        int x = currentNode.X;
        int y = currentNode.Y;

        if (!cells[x, y].LeftWall &&
                x > 0 &&
                currentNode.prevDir != Vector2Int.right)
        {
            cells[x - 1, y].prevDir = Vector2Int.left;
            dirs.Push(cells[x - 1, y]);
        }
        if (!cells[x, y].BottomWall &&
            y > 0 &&
            currentNode.prevDir != Vector2Int.up)
        {
            cells[x, y - 1].prevDir = Vector2Int.down;
            dirs.Push(cells[x, y - 1]);
        }
        if (x < width - 1 &&
            !cells[x, y].RightWall &&
            currentNode.prevDir != Vector2Int.left)
        {
            cells[x + 1, y].prevDir = Vector2Int.right;
            dirs.Push(cells[x + 1, y]);
        }
        if (currentNode.Y < height - 1 &&
            !cells[x, y].UpperWall &&
            currentNode.prevDir != Vector2Int.down)
        {
            cells[x, y + 1].prevDir = Vector2Int.up;
            dirs.Push(cells[x, y + 1]);
        }
        return dirs;
    }

    private PerfectMazeGeneratorCell GetNextCell(PerfectMazeGeneratorCell[,] cells, PerfectMazeGeneratorCell currentCell, List<Vector2Int> path)
    {
        int x = currentCell.X;
        int y = currentCell.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 &&
            currentCell.prevDir != Vector2Int.right)
        {
            path.Add(Vector2Int.left);
            currentCell = cells[x - 1, y];
            currentCell.prevDir = Vector2Int.left;
        }
        else if (!cells[x, y].BottomWall &&
            y > 0 &&
            currentCell.prevDir != Vector2Int.up)
        {
            path.Add(Vector2Int.down);
            currentCell = cells[x, y - 1];
            currentCell.prevDir = Vector2Int.down;
        }
        else if (x < width - 1 &&
            !cells[x, y].RightWall &&
            currentCell.prevDir != Vector2Int.left)
        {
            path.Add(Vector2Int.right);
            currentCell = cells[x + 1, y];
            currentCell.prevDir = Vector2Int.right;
        }
        else if (y < height - 1 &&
            !cells[x, y].UpperWall &&
            currentCell.prevDir != Vector2Int.down)
        {
            path.Add(Vector2Int.up);
            currentCell = cells[x, y + 1];
            currentCell.prevDir = Vector2Int.up;
        }
        return currentCell;
    }
}