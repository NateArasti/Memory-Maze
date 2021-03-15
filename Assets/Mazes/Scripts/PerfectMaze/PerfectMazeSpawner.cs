using UnityEngine;

public class PerfectMazeSpawner : MazeSpawner
{
    [SerializeField] protected GameObject Finish;

    public override Vector3 cellSize3D { get; } = new Vector3(10, 0, 10);
    public override Vector3 cellSize2D { get; } = new Vector3(1, 1, 0);

    protected override MazeGenerator generator => new PerfectMazeGenerator();

    protected override void CreateFinish(MazeGeneratorCell finishCell)
    {
        var x = finishCell.X;
        var y = finishCell.Y;
        const int delta = 2;
        if (x == 0)
            Instantiate(Finish,
                new Vector3((x - delta) * cellSize3D.x, y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes),
                Quaternion.identity);
        else if (y == 0)
            Instantiate(Finish,
                new Vector3(x * cellSize3D.x, y * cellSize3D.y, (y - delta) * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0, -90, 0));
        else if (x == width - 1)
            Instantiate(Finish,
                new Vector3((x + delta) * cellSize3D.x, y * cellSize3D.y, y * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0, 180, 0));
        else
            Instantiate(Finish,
                new Vector3(x * cellSize3D.x, y * cellSize3D.y, (y + delta) * cellSize3D.z + distanceBetweenMazes),
                Quaternion.Euler(0, 90, 0));
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector2(width / 2f, height / 2f - distanceBetweenMazes);
        Camera.main.orthographicSize += Mathf.Max(height, width) / 1.5f;
    }

    protected override void SpawnMazeCells(MazeGeneratorCell cell)
    {
        CreateCell(Cell2D,
            cell,
            new Vector3(cell.X * cellSize2D.x, cell.Y * cellSize2D.y - distanceBetweenMazes, cell.Y * cellSize2D.z));
        CreateCell(Cell3D,
            cell,
            new Vector3(cell.X * cellSize3D.x, cell.Y * cellSize3D.y, cell.Y * cellSize3D.z + distanceBetweenMazes));
    }
}