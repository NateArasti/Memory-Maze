using UnityEngine;

public class PerfectMazeSpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject Cell2D;
    public GameObject Cell3D;
    public GameObject Floor;
    public GameObject Finish3D;
    public Vector3 cellSize3D = new Vector3(10,0,10);
    public Vector3 cellSize2D = new Vector3(1, 1, 0);
    public int distanceBetweenMazes = 50;
    public PerfectMaze Maze;

    [Header("Set Dynamically")]
    public int width;
    public int height;

    void Awake()
    {
        var generator = new PerfectMazeGenerator();
        width = generator.width;
        height = generator.height;
        Camera.main.transform.position = new Vector2(width / 2f, height / 2f - distanceBetweenMazes);
        Camera.main.orthographicSize += Mathf.Max(height,width) / 1.5f;

        Maze = generator.GenerateMaze();
        var cells = Maze.cells;
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                CreateCell(Cell2D,
                    cells[x, y],
                    new Vector3(x * cellSize2D.x, y * cellSize2D.y - distanceBetweenMazes, y * cellSize2D.z));
                CreateCell(Cell3D,
                    cells[x, y],
                    new Vector3(x * cellSize3D.x, y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes));
            }
        }
        var floor = Instantiate(Floor,
            new Vector3(cellSize3D.x * (width / 2), 0, cellSize3D.z * (height / 2) + distanceBetweenMazes),
            Quaternion.identity);
        floor.transform.localScale = new Vector3(cellSize3D.x * (5 + width), 0.1f, cellSize3D.z * (5 + height));
        CreateFinish(Maze.finishPosition);
    }

    private void CreateCell(GameObject cellPrefab, PerfectMazeGeneratorCell currentCell, Vector3 coordinates)
    {
        PerfectMazeCell c = Instantiate(cellPrefab, coordinates,
                    Quaternion.identity).GetComponent<PerfectMazeCell>();
        c.bottomWall.SetActive(currentCell.BottomWall);
        c.leftWall.SetActive(currentCell.LeftWall);
        c.rightWall.SetActive(currentCell.RightWall);
        c.upperWall.SetActive(currentCell.UpperWall);
    }

    private void CreateFinish(PerfectMazeGeneratorCell finishCell)
    {
        var x = finishCell.X;
        var y = finishCell.Y;
        var delta = 2;
        if(x == 0)
        {
            Instantiate(Finish3D,
                new Vector3((x - delta) * cellSize3D.x, y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes),
                Quaternion.identity);
        }
        else if (y == 0)
        {
            Instantiate(Finish3D,
                new Vector3(x * cellSize3D.x, y * cellSize3D.y, (y - delta) * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0,-90,0));
        }
        else if (x == width - 1)
        {
            Instantiate(Finish3D,
                new Vector3((x + delta) * cellSize3D.x, y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0, 180, 0));
        }
        else
        {
            Instantiate(Finish3D,
                new Vector3(x * cellSize3D.x, y * cellSize3D.y, (y + delta) * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0, 90, 0));
        }
    }
}
