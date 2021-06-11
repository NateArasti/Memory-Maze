using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once IdentifierTypo
public abstract class MazeSpawner : MonoBehaviour
{
#pragma warning disable 649
    [Header("Finish")] 
    [SerializeField] protected GameObject Finish;

    [Header("PlayerMovement")] 
    [SerializeField] private GameObject player2D;

    [SerializeField] private Vector3 player2DPositionDelta;
    [SerializeField] private Vector3 player2DScale;

    [Header("Floor")] 
    [SerializeField] private GameObject floor;

    [SerializeField] private float floorDelta;

    [Header("Cells")] 
    [SerializeField] private GameObject cell2DPrefab;

    [SerializeField] private GameObject cell3DPrefab;

    [Header("StoryScroll")] 
    [SerializeField] private GameObject scrollPrefab;

    public int Width { get; private set; }
    public int Height { get; private set; }
    protected int DistanceBetweenMazes => 50;
    public Maze Maze { get; private set; }
    protected abstract MazeGenerator Generator { get; }

    private GameObject finishCell;
    private bool storyIsNeededToBePlaced = true;

    private void Awake()
    {
        storyIsNeededToBePlaced = !StoriesStorage.AllStoriesCollected && Random.value > 0.6f;
        Width = Generator.Width;
        Height = Generator.Height;
        SetCamera();

        Maze = Generator.Maze;

        SpawnMaze(Maze.StartCell);

        var floorCopy = Instantiate(
            floor,
            new Vector3(5 * Width, DistanceBetweenMazes - floorDelta, 5 * Height),
            Quaternion.identity);
        var maxLength = Mathf.Max(Width, Height);
        floorCopy.transform.localScale = new Vector3(50 * maxLength, 0.1f, 50 * maxLength);
        floorCopy.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(15 * maxLength, 15 * maxLength);

        Instantiate(player2D, Maze.StartCell.Cell2DPosition + player2DPositionDelta, Quaternion.identity)
            .transform.localScale = player2DScale;

        CreateFinish(finishCell);
    }

    private void SpawnMaze(MazeCell startPosition)
    {
        var spawnedWalls = new HashSet<Vector3>();

        var currentCell = startPosition;
        var check = currentCell.Visited;
        currentCell.Visited = !check;

        var cellStack = new Stack<MazeCell>();
        do
        {
            SpawnCell(currentCell, spawnedWalls);

            var unvisitedNeighbors = MazeCell.GetNeighborsList(currentCell, check);
            if (unvisitedNeighbors.Count > 0)
            {
                var chosenCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                chosenCell.Visited = !check;
                cellStack.Push(chosenCell);
                currentCell = chosenCell;
            }
            else
            {
                currentCell = cellStack.Pop();
            }
        } while (cellStack.Count > 0);
    }

    private void SpawnCell(MazeCell cell, HashSet<Vector3> spawnedWalls)
    {
        Instantiate(cell2DPrefab, cell.Cell2DPosition, Quaternion.identity)
            .GetComponent<Cell>()
            .SetWalls(cell.Walls);
        var cell3d = Instantiate(cell3DPrefab, cell.Cell3DPosition, Quaternion.identity);
        foreach (var wallTransform in cell3d.GetComponent<Cell>().GetWallsTransforms())
            if (spawnedWalls.Contains(wallTransform.position)) Destroy(wallTransform.gameObject);
            else spawnedWalls.Add(wallTransform.position);
        cell3d.GetComponent<Cell>().SetWalls(cell.Walls);
        if (cell == Maze.FinishCell)
        {
            finishCell = cell3d;
            return;
        }

        if (cell == Maze.StartCell || cell.Walls.Count(wall => !wall.Value) != 1 || !storyIsNeededToBePlaced ||
            Random.value < 0.95f) return;
        Instantiate(scrollPrefab, cell.Cell3DPosition, Quaternion.identity)
            .GetComponent<ScrollCollectable>().Length = cell.DistanceFromStart;
        storyIsNeededToBePlaced = false;
    }

    protected abstract void SetCamera();

    private void CreateFinish(GameObject finishCellGameObject)
    {
        foreach (var neighbor in Maze.FinishCell.Neighbors.Where(neighbor => neighbor.Value == null))
        {
            var trans = finishCellGameObject.GetComponent<Cell>().GetWallTransform(neighbor.Key);
            var finish = Instantiate(Finish, trans.position,
                Quaternion.Euler(0, trans.eulerAngles.y, 0));
            finish.transform.GetChild(0).localScale = new Vector3(trans.localScale.x / Finish.transform.localScale.x,
                trans.localScale.y / Finish.transform.localScale.y, 0.01f);
            var finishScale = finish.transform.GetChild(0).localScale;
            finish.GetComponent<BoxCollider>().size = new Vector3(finishScale.x, finishScale.y, 1);
            return;
        }
    }
}