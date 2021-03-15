using UnityEngine;

public abstract class MazeSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject Cell3D;
    [SerializeField] protected GameObject Cell2D;
    [SerializeField] protected GameObject Floor;
    public abstract Vector3 cellSize3D { get; }
    public abstract Vector3 cellSize2D { get; }
    public int width { get; protected set; }
    public int height { get; protected set; }

    public int distanceBetweenMazes { get => 50; } 
    public Maze Maze { get; protected set; }
    protected abstract MazeGenerator generator { get; }

    private void Awake()
    {
        width = generator.width;
        height = generator.height;
        SetCamera();

        Maze = generator.Maze;
        var cells = Maze.Cells;
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                SpawnMazeCells(cells[x, y]);
            }
        }
        var floor = Instantiate(Floor,
            new Vector3(cellSize3D.x * (width / 2), 0, cellSize3D.z * (height / 2) + distanceBetweenMazes),
            Quaternion.identity);
        floor.transform.localScale = new Vector3(cellSize3D.x * (5 + width), 0.1f, cellSize3D.z * (5 + height));
        CreateFinish(Maze.FinishPosition);
    }

    protected abstract void SetCamera();
    protected abstract void CreateFinish(MazeGeneratorCell finishCell);
    protected abstract void SpawnMazeCells(MazeGeneratorCell cell);

    protected void CreateCell(GameObject cellPrefab, MazeGeneratorCell currentCell, Vector3 coordinates)
    {
        MazeCell c = Instantiate(cellPrefab, coordinates,
                    Quaternion.identity).GetComponent<MazeCell>();
        if(c.BottomWall != null) c.BottomWall.SetActive(currentCell.BottomWall);
        if(c.LeftWall != null) c.LeftWall.SetActive(currentCell.LeftWall);
        if(c.RightWall != null) c.RightWall.SetActive(currentCell.RightWall);
        if(c.UpperWall != null) c.UpperWall.SetActive(currentCell.UpperWall);
    }
}