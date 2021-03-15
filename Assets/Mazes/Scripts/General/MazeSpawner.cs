using UnityEngine;

public abstract class MazeSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject Cell3D;
    [SerializeField] protected GameObject Cell2D;
    [SerializeField] protected GameObject Floor;
    public abstract Vector3 CellSize3D { get; }
    public abstract Vector3 CellSize2D { get; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    protected int DistanceBetweenMazes => 50;
    public Maze Maze { get; private set; }
    protected abstract MazeGenerator Generator { get; }

    private void Awake()
    {
        Width = Generator.width;
        Height = Generator.height;
        SetCamera();

        Maze = Generator.Maze;
        var cells = Maze.Cells;
        for (var x = 0; x < Width; ++x)
        for (var y = 0; y < Height; ++y)
            SpawnMazeCells(cells[x, y]);

        var floor = Instantiate(Floor,
            new Vector3(CellSize3D.x * (Width / 2), 0, CellSize3D.z * (Height / 2) + DistanceBetweenMazes),
            Quaternion.identity);
        floor.transform.localScale = new Vector3(CellSize3D.x * (5 + Width), 0.1f, CellSize3D.z * (5 + Height));
        CreateFinish(Maze.FinishPosition);
    }

    protected abstract void SetCamera();
    protected abstract void CreateFinish(MazeGeneratorCell finishCell);
    protected abstract void SpawnMazeCells(MazeGeneratorCell cell);

    protected void CreateCell(GameObject cellPrefab, MazeGeneratorCell currentCell, Vector3 coordinates)
    {
        var c = Instantiate(cellPrefab, coordinates,
            Quaternion.identity).GetComponent<MazeCell>();
        if (c.BottomWall != null) c.BottomWall.SetActive(currentCell.BottomWall);
        if (c.LeftWall != null) c.LeftWall.SetActive(currentCell.LeftWall);
        if (c.RightWall != null) c.RightWall.SetActive(currentCell.RightWall);
        if (c.UpperWall != null) c.UpperWall.SetActive(currentCell.UpperWall);
    }
}