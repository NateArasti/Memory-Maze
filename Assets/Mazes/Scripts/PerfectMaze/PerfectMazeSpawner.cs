using UnityEngine;

public class PerfectMazeSpawner : MazeSpawner
{
    [SerializeField] protected GameObject Finish;

    public override Vector3 CellSize3D { get; } = new Vector3(10, 0, 10);
    public override Vector3 CellSize2D { get; } = new Vector3(1, 1, 0);

    protected override MazeGenerator Generator => new PerfectMazeGenerator();

    protected override void CreateFinish(MazeGeneratorCell finishCell)
    {
        var x = finishCell.X;
        var y = finishCell.Y;
        const int delta = 2;
        if (x == 0)
            Instantiate(Finish,
                new Vector3((x - delta) * CellSize3D.x, y * CellSize3D.y, y * CellSize3D.z + DistanceBetweenMazes),
                Quaternion.identity);
        else if (y == 0)
            Instantiate(Finish,
                new Vector3(x * CellSize3D.x, y * CellSize3D.y, (y - delta) * CellSize3D.z + DistanceBetweenMazes),
                Quaternion.Euler(0, -90, 0));
        else if (x == Width - 1)
            Instantiate(Finish,
                new Vector3((x + delta) * CellSize3D.x, y * CellSize3D.y, y * CellSize3D.z + DistanceBetweenMazes),
                Quaternion.Euler(0, 180, 0));
        else
            Instantiate(Finish,
                new Vector3(x * CellSize3D.x, y * CellSize3D.y, (y + delta) * CellSize3D.z + DistanceBetweenMazes),
                Quaternion.Euler(0, 90, 0));
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector2(Width / 2f, Height / 2f - DistanceBetweenMazes);
        Camera.main.orthographicSize += Mathf.Max(Height, Width) / 1.5f;
    }

    protected override void SpawnMazeCells(MazeGeneratorCell cell)
    {
        CreateCell(Cell2D,
            cell,
            new Vector3(cell.X * CellSize2D.x, cell.Y * CellSize2D.y - DistanceBetweenMazes, cell.Y * CellSize2D.z));
        CreateCell(Cell3D,
            cell,
            new Vector3(cell.X * CellSize3D.x, cell.Y * CellSize3D.y, cell.Y * CellSize3D.z + DistanceBetweenMazes));
    }
}