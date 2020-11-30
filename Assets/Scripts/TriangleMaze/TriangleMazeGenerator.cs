using System.Collections.Generic;
using UnityEngine;

public class TriangleMazeGenerator
{
    public int height = PlayerPrefs.HasKey("Side") ? PlayerPrefs.GetInt("Side") : 5;
    public int width = (PlayerPrefs.HasKey("Side") ? PlayerPrefs.GetInt("Side") : 5) * 2 - 1;


    public TriangleMaze GenerateMaze()
    {
        var cells = new TriangleMazeGeneratorCell[width, height];

        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                if (y <= x && x < width - y) 
                    cells[x, y] = new TriangleMazeGeneratorCell { X = x, Y = y };
                else
                    cells[x, y] = new TriangleMazeGeneratorCell { X = -1, Y = -1 };
            }
        }

        RemoveWallWithBackTracker(cells);

        var maze = new TriangleMaze();
        maze.cells = cells;
        maze.startPosition = cells[0, 0];
        maze.nodes = GetAllNodes(maze);
        maze.finishPosition = MakeExit(cells);

        return maze;
    }

    private void RemoveWallWithBackTracker(TriangleMazeGeneratorCell[,] cells)
    {
        TriangleMazeGeneratorCell currentCell = cells[0, 0];
        currentCell.Visited = true;
        currentCell.DistanceFromStart = 0;

        var cellStack = new Stack<TriangleMazeGeneratorCell>();
        do
        {
            var unvisitedNeighbours = new List<TriangleMazeGeneratorCell>();

            int x = currentCell.X;
            int y = currentCell.Y;

            if (x > 0 && !cells[x - 1, y].Visited && cells[x - 1, y].X != -1) 
                unvisitedNeighbours.Add(cells[x - 1, y]);
            if (x < width - 1 && !cells[x + 1, y].Visited && cells[x + 1, y].X != -1)
                unvisitedNeighbours.Add(cells[x + 1, y]);
            if ((x + y) % 2 == 0 && y > 0 && !cells[x, y - 1].Visited && cells[x, y - 1].X != -1) 
                unvisitedNeighbours.Add(cells[x, y - 1]);
            if ((x + y) % 2 == 1 && y < height - 1 && !cells[x, y + 1].Visited && cells[x, y + 1].X != -1)
                unvisitedNeighbours.Add(cells[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                TriangleMazeGeneratorCell chosenCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(currentCell, chosenCell);

                chosenCell.Visited = true;
                cellStack.Push(chosenCell);
                chosenCell.DistanceFromStart = currentCell.DistanceFromStart + 1;
                currentCell = chosenCell;
            }
            else
                currentCell = cellStack.Pop();

        } while (cellStack.Count > 0);
    }
    private void RemoveWall(TriangleMazeGeneratorCell cellA, TriangleMazeGeneratorCell cellB)
    {
        if (cellA.X == cellB.X)
        {
            if (cellA.Y > cellB.Y)
            {
                cellA.BottomWall = false;
                cellB.BottomWall = false;
            }
            else
            {
                cellA.BottomWall = false;
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

    private TriangleMazeGeneratorCell MakeExit(TriangleMazeGeneratorCell[,] cells)
    {
        TriangleMazeGeneratorCell furthest = cells[0, 0];
        for (var y = 0; y < height; ++y)
        {
            if (cells[y, y].DistanceFromStart > furthest.DistanceFromStart && cells[y, y].isDeadEnd)
                furthest = cells[y, y];
            if (cells[width - y - 1, y].DistanceFromStart > furthest.DistanceFromStart && cells[width - y - 1, y].isDeadEnd)
                furthest = cells[width - y - 1, y];
        }
        for(var x = 0; x < width; ++x)
        {
            if (cells[x, 0].DistanceFromStart > furthest.DistanceFromStart && cells[x, 0].isDeadEnd)
                furthest = cells[x, 0];
        }

        if (furthest.X == 0 || cells[furthest.X-1,furthest.Y].X == -1) furthest.LeftWall = false;
        else if (furthest.Y == 0) furthest.BottomWall = false;
        else cells[furthest.X, furthest.Y].RightWall = false;

        return new TriangleMazeGeneratorCell { X = furthest.X, Y = furthest.Y };
    }

    private Dictionary<TriangleMazeGeneratorCell, Dictionary<TriangleMazeGeneratorCell, List<Vector2Int>>> GetAllNodes(TriangleMaze maze)
    {
        var nodes = new Dictionary<TriangleMazeGeneratorCell, Dictionary<TriangleMazeGeneratorCell, List<Vector2Int>>>();
        var cells = maze.cells;
        var stackOfNodes = new Stack<TriangleMazeGeneratorCell>();
        maze.startPosition.prevDir = Vector2Int.zero;
        stackOfNodes.Push(maze.startPosition);
        var unvistedDirections = new Stack<TriangleMazeGeneratorCell>();
        do
        {
            var currentNode = stackOfNodes.Pop();
            unvistedDirections.Clear();
            var nextNodes = new Dictionary<TriangleMazeGeneratorCell, List<Vector2Int>>();

            unvistedDirections = FindPossibleDirections(cells, currentNode);

            while (unvistedDirections.Count > 0)
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

    private bool CheckIfNode(TriangleMazeGeneratorCell currentCell)
    {
        int count = 0;

        if (!currentCell.LeftWall) count++;
        if (!currentCell.BottomWall) count++;
        if (!currentCell.RightWall) count++;

        return count != 2 ? true : false;
    }

    private Stack<TriangleMazeGeneratorCell> FindPossibleDirections(TriangleMazeGeneratorCell[,] cells, TriangleMazeGeneratorCell currentNode)
    {
        var dirs = new Stack<TriangleMazeGeneratorCell>();
        int x = currentNode.X;
        int y = currentNode.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 && cells[x - 1, y].X != -1 &&
            currentNode.prevDir != Vector2Int.right)
        {
            cells[x - 1, y].prevDir = Vector2Int.left;
            dirs.Push(cells[x - 1, y]);
        }
        if (!cells[x, y].BottomWall &&
            y > 0 && (x + y) % 2 == 0 &&
            currentNode.prevDir != Vector2Int.up)
        {
            cells[x, y - 1].prevDir = Vector2Int.down;
            dirs.Push(cells[x, y - 1]);
        }
        if (!cells[x, y].RightWall &&
            x < width - 1 && cells[x + 1, y].X != -1 &&
            currentNode.prevDir != Vector2Int.left)
        {
            cells[x + 1, y].prevDir = Vector2Int.right;
            dirs.Push(cells[x + 1, y]);
        }
        if (!cells[x, y].BottomWall &&
            currentNode.Y < height - 1 && (x + y) % 2 == 1 &&
            currentNode.prevDir != Vector2Int.down)
        {
            cells[x, y + 1].prevDir = Vector2Int.up;
            dirs.Push(cells[x, y + 1]);
        }
        return dirs;
    }

    private TriangleMazeGeneratorCell GetNextCell(TriangleMazeGeneratorCell[,] cells, TriangleMazeGeneratorCell currentCell, List<Vector2Int> path)
    {
        int x = currentCell.X;
        int y = currentCell.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 && cells[x - 1, y].X != -1 &&
            currentCell.prevDir != Vector2Int.right)
        {
            path.Add(Vector2Int.left);
            currentCell = cells[x - 1, y];
            currentCell.prevDir = Vector2Int.left;
        }
        else if (!cells[x, y].BottomWall &&
            y > 0 && (x + y) % 2 == 0 &&
            currentCell.prevDir != Vector2Int.up)
        {
            path.Add(Vector2Int.down);
            currentCell = cells[x, y - 1];
            currentCell.prevDir = Vector2Int.down;
        }
        else if (!cells[x, y].RightWall &&
            x < width - 1 && cells[x + 1, y].X != -1 &&
            currentCell.prevDir != Vector2Int.left)
        {
            path.Add(Vector2Int.right);
            currentCell = cells[x + 1, y];
            currentCell.prevDir = Vector2Int.right;
        }
        else if (!cells[x, y].BottomWall &&
            y < height - 1 && (x + y) % 2 == 1 &&
            currentCell.prevDir != Vector2Int.down)
        {
            path.Add(Vector2Int.up);
            currentCell = cells[x, y + 1];
            currentCell.prevDir = Vector2Int.up;
        }
        return currentCell;
    }
}
