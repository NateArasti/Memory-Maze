using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGenerator
{
    public readonly Maze Maze;

    public int width { get; protected set; }
    public int height { get; protected set; }

    protected MazeGeneratorCell[,] cells;

    protected MazeGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new MazeGeneratorCell[width, height];
        Maze = GenerateMaze();
    }

    protected abstract void FillTheMaze();

    private Maze GenerateMaze()
    {
        FillTheMaze();

        RemoveWallsWithBackracker();

        var maze = new Maze
        {
            Cells = cells,
            StartPosition = cells[0, 0]
        };
        maze.Nodes = GetAllNodes(maze);
        maze.FinishPosition = MakeExit();

        return maze;
    }

    protected abstract void FillNeighboursList(
        List<MazeGeneratorCell> unvisitedNeighbours,
        MazeGeneratorCell currentCell);

    private void RemoveWallsWithBackracker()
    {
        var currentCell = cells[0, 0];
        currentCell.Visited = true;
        currentCell.DistanceFromStart = 0;

        var cellStack = new Stack<MazeGeneratorCell>();

        do
        {
            var unvisitedNeighbours = new List<MazeGeneratorCell>();

            FillNeighboursList(unvisitedNeighbours, currentCell);

            if (unvisitedNeighbours.Count > 0)
            {
                var chosenCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(currentCell, chosenCell);

                chosenCell.Visited = true;
                cellStack.Push(chosenCell);
                chosenCell.DistanceFromStart = currentCell.DistanceFromStart + 1;
                currentCell = chosenCell;
            }
            else
            {
                currentCell = cellStack.Pop();
            }
        } while (cellStack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell cellA, MazeGeneratorCell cellB)
    {
        if (cellA.X == cellB.X)
        {
            if (cellA.Y > cellB.Y)
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

    protected abstract MazeGeneratorCell MakeExit();

    //Возвращает узлы(развилки или тупики) лабиринта
    private Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>> GetAllNodes(Maze maze)
    {
        var nodes = new Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>>();
        var stackOfNodes = new Stack<MazeGeneratorCell>();
        maze.StartPosition.PreviousDirection = Vector2Int.zero;
        stackOfNodes.Push(maze.StartPosition);
        var unvisitedDirections = new Stack<MazeGeneratorCell>();
        do
        {
            var currentNode = stackOfNodes.Pop();
            unvisitedDirections.Clear();
            var nextNodes = new Dictionary<MazeGeneratorCell, List<Vector2Int>>();
            unvisitedDirections = FindPossibleDirections(currentNode);
            while (unvisitedDirections.Count > 0)
            {
                var currentCell = unvisitedDirections.Pop();
                var count = 0;
                var path = new List<Vector2Int> {currentCell.PreviousDirection};
                while (!CheckIfNode(currentCell))
                {
                    currentCell = GetNextCell(currentCell, path);
                    //todo: Убрать костыль!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    count++;
                    if (count > width * height) break;
                }

                stackOfNodes.Push(currentCell);
                nextNodes[currentCell] = path;
                currentCell.PreviousNode = currentNode;
            }

            nodes[currentNode] = nextNodes;
            if (nextNodes.Count == 0)
                currentNode.IsDeadEnd = true;
        } while (stackOfNodes.Count > 0);

        return nodes;
    }

    private bool CheckIfNode(MazeGeneratorCell currentCell)
    {
        var count = 0;

        if (!currentCell.LeftWall) count++;
        if (!currentCell.BottomWall) count++;
        if (!currentCell.RightWall) count++;
        if (!currentCell.UpperWall) count++;

        return count != 2;
    }

    private Stack<MazeGeneratorCell> FindPossibleDirections(
        MazeGeneratorCell currentNode)
    {
        var dirs = new Stack<MazeGeneratorCell>();
        var x = currentNode.X;
        var y = currentNode.Y;

        if (!cells[x, y].LeftWall && x > 0 && cells[x - 1, y] != null &&
            currentNode.PreviousDirection != Vector2Int.right)
        {
            cells[x - 1, y].PreviousDirection = Vector2Int.left;
            dirs.Push(cells[x - 1, y]);
        }

        if (!cells[x, y].BottomWall && y > 0 && currentNode.PreviousDirection != Vector2Int.up)
        {
            cells[x, y - 1].PreviousDirection = Vector2Int.down;
            dirs.Push(cells[x, y - 1]);
        }

        if (!cells[x, y].RightWall && x < width - 1 && cells[x + 1, y] != null &&
            currentNode.PreviousDirection != Vector2Int.left)
        {
            cells[x + 1, y].PreviousDirection = Vector2Int.right;
            dirs.Push(cells[x + 1, y]);
        }

        if (!cells[x, y].UpperWall && currentNode.Y < height - 1 && currentNode.PreviousDirection != Vector2Int.down)
        {
            cells[x, y + 1].PreviousDirection = Vector2Int.up;
            dirs.Push(cells[x, y + 1]);
        }

        return dirs;
    }

    private MazeGeneratorCell GetNextCell(MazeGeneratorCell currentCell, List<Vector2Int> path)
    {
        var x = currentCell.X;
        var y = currentCell.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 && cells[x - 1, y] != null &&
            currentCell.PreviousDirection != Vector2Int.right)
        {
            path.Add(Vector2Int.left);
            currentCell = cells[x - 1, y];
            currentCell.PreviousDirection = Vector2Int.left;
        }
        else if (!cells[x, y].BottomWall &&
                 y > 0 &&
                 currentCell.PreviousDirection != Vector2Int.up)
        {
            path.Add(Vector2Int.down);
            currentCell = cells[x, y - 1];
            currentCell.PreviousDirection = Vector2Int.down;
        }
        else if (!cells[x, y].RightWall &&
                 x < width - 1 && cells[x + 1, y] != null &&
                 currentCell.PreviousDirection != Vector2Int.left)
        {
            path.Add(Vector2Int.right);
            currentCell = cells[x + 1, y];
            currentCell.PreviousDirection = Vector2Int.right;
        }
        else if (!cells[x, y].UpperWall &&
                 currentCell.Y < height - 1 &&
                 currentCell.PreviousDirection != Vector2Int.down)
        {
            path.Add(Vector2Int.up);
            currentCell = cells[x, y + 1];
            currentCell.PreviousDirection = Vector2Int.up;
        }

        return currentCell;
    }
}