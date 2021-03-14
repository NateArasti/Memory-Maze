using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGenerator
{
    public readonly Maze Maze;

    public int width { get; protected set; }
    public int height { get; protected set; }

    protected MazeGeneratorCell[,] cells;

    public MazeGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new MazeGeneratorCell[width, height];
        Maze = GenerateMaze();
    }

    protected abstract void FillTheMaze();

    public Maze GenerateMaze()
    {
        FillTheMaze();

        RemoveWallsWithBackracker();

        var maze = new Maze();
        maze.cells = cells;
        maze.startPosition = cells[0,0];
        maze.nodes = GetAllNodes(maze);
        maze.finishPosition = MakeExit();

        return maze;
    }

    protected abstract void FillNeighboursList(
        List<MazeGeneratorCell> unvisitedNeighbours, 
        MazeGeneratorCell currentCell);

    private void RemoveWallsWithBackracker()
    {
        MazeGeneratorCell currentCell = cells[0, 0];
        currentCell.Visited = true;
        currentCell.DistanceFromStart = 0;

        var cellStack = new Stack<MazeGeneratorCell>();

        do
        {
            var unvisitedNeighbours = new List<MazeGeneratorCell>();

            FillNeighboursList(unvisitedNeighbours, currentCell);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosenCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
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

    protected void RemoveWall(MazeGeneratorCell cellA, MazeGeneratorCell cellB)
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

    //���������� ����(�������� ��� ������) ���������
    private Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>>
        GetAllNodes(Maze maze)
    {
        var nodes = new Dictionary<MazeGeneratorCell, Dictionary<MazeGeneratorCell, List<Vector2Int>>>();
        var stackOfNodes = new Stack<MazeGeneratorCell>();
        maze.startPosition.prevDir = Vector2Int.zero;
        stackOfNodes.Push(maze.startPosition);
        var unvistedDirections = new Stack<MazeGeneratorCell>();
        do
        {
            var currentNode = stackOfNodes.Pop();
            unvistedDirections.Clear();
            var nextNodes = new Dictionary<MazeGeneratorCell, List<Vector2Int>>();

            unvistedDirections = FindPossibleDirections(currentNode);

            while (unvistedDirections.Count > 0)
            {
                var currentCell = unvistedDirections.Pop();
                int count = 0;
                var path = new List<Vector2Int>() { currentCell.prevDir };
                while (!CheckIfNode(currentCell))
                {
                    currentCell = GetNextCell(currentCell, path);
                    //������ �������!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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

    private bool CheckIfNode(MazeGeneratorCell currentCell)
    {
        var count = 0;

        if (!currentCell.LeftWall) count++;
        if (!currentCell.BottomWall) count++;
        if (!currentCell.RightWall) count++;
        if (!currentCell.UpperWall) count++;

        return count != 2 ? true : false;
    }

    private Stack<MazeGeneratorCell> FindPossibleDirections(
        MazeGeneratorCell currentNode)
    {
        var dirs = new Stack<MazeGeneratorCell>();
        int x = currentNode.X;
        int y = currentNode.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 && cells[x - 1, y] != null &&
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
        if (!cells[x, y].RightWall &&
            x < width - 1 && cells[x + 1, y] != null &&
            currentNode.prevDir != Vector2Int.left)
        {
            cells[x + 1, y].prevDir = Vector2Int.right;
            dirs.Push(cells[x + 1, y]);
        }
        if (!cells[x, y].UpperWall &&
            currentNode.Y < height - 1 && 
            currentNode.prevDir != Vector2Int.down)
        {
            cells[x, y + 1].prevDir = Vector2Int.up;
            dirs.Push(cells[x, y + 1]);
        }
        return dirs;
    }

    private MazeGeneratorCell GetNextCell(
        MazeGeneratorCell currentCell, 
        List<Vector2Int> path)
    {
        int x = currentCell.X;
        int y = currentCell.Y;

        if (!cells[x, y].LeftWall &&
            x > 0 && cells[x - 1, y] != null &&
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
        else if (!cells[x, y].RightWall &&
            x < width - 1 && cells[x + 1, y] != null &&
            currentCell.prevDir != Vector2Int.left)
        {
            path.Add(Vector2Int.right);
            currentCell = cells[x + 1, y];
            currentCell.prevDir = Vector2Int.right;
        }
        else if (!cells[x, y].UpperWall &&
            currentCell.Y < height - 1 &&
            currentCell.prevDir != Vector2Int.down)
        {
            path.Add(Vector2Int.up);
            currentCell = cells[x, y + 1];
            currentCell.prevDir = Vector2Int.up;
        }
        return currentCell;
    }
}