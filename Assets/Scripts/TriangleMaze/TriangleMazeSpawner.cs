using UnityEngine;

public class TriangleMazeSpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject Cell2D;
    public GameObject Cell3D;
    public GameObject Floor;
    public Vector3 cellSize3D = new Vector3(6.6f, 0, 10);
    public Vector3 cellSize2D = new Vector3(1, 1, 0);
    public int distanceBetweenMazes = 50;
    public TriangleMaze Maze;

    [Header("Set Dynamically")]
    public int width;
    public int height;

    void Start()
    {
        var generator = new TriangleMazeGenerator();
        width = generator.width;
        height = generator.height;
        var sideLength = PlayerPrefs.GetInt("Side");

        Camera.main.transform.position = new Vector3(sideLength / 2f, sideLength * Mathf.Sqrt(3) / 4f - distanceBetweenMazes, -2);
        Camera.main.orthographicSize += sideLength / 2f;
        
        Maze = generator.GenerateMaze();
        var cells = Maze.cells;
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                if (cells[x, y].X != -1)
                {
                    if ((x + y) % 2 == 0) 
                    {
                        CreateCell(Cell2D,
                            cells[x, y],
                            new Vector3(x * cellSize2D.x, y * cellSize2D.y - distanceBetweenMazes, y * cellSize2D.z));
                        CreateCell(Cell3D,
                            cells[x, y],
                            new Vector3(x * cellSize3D.x, 2 + y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes));
                    }
                }
            }
        }
        var floor = Instantiate(Floor,
            new Vector3(cellSize3D.x * (width / 2), 0, cellSize3D.z * (height / 2) + distanceBetweenMazes),
            Quaternion.identity);
        floor.transform.localScale = new Vector3(cellSize3D.x * (5 + width), 0.1f, cellSize3D.z * (5 + height));

        var finishX = Maze.finishPosition.X;
        var finishY = Maze.finishPosition.Y;
        if (finishY == 0)
        {
            var finish = Instantiate(Cell3D,
                new Vector3(Maze.finishPosition.X * cellSize3D.x, 2f, (Maze.finishPosition.Y - 1) * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<TriangleMazeCell>();
            finish.bottomWall.SetActive(false);
        }
        else if (finishX < width && cells[finishX + 1, finishY].X == -1)
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.finishPosition.X + 1) * cellSize3D.x, 2f, Maze.finishPosition.Y * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<TriangleMazeCell>();
            finish.rightWall.SetActive(false);
        }
        else
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.finishPosition.X - 1) * cellSize3D.x, 2f, Maze.finishPosition.Y * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<TriangleMazeCell>();
            finish.leftWall.SetActive(false);
        }
    }

    private void CreateCell(GameObject cellPrefab, TriangleMazeGeneratorCell currentCell, Vector3 coordinates)
    {
        TriangleMazeCell c = Instantiate(cellPrefab, coordinates,
                        Quaternion.identity).GetComponent<TriangleMazeCell>();
        c.bottomWall.SetActive(currentCell.BottomWall);
        c.leftWall.SetActive(currentCell.LeftWall);
        c.rightWall.SetActive(currentCell.RightWall);
    }
}
