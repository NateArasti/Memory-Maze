using UnityEngine;

public class TriangleMazeSpawner : MazeSpawner
{
    public override Vector3 CellSize3D { get; } = new Vector3(6, 0, 10.3923f);
    public override Vector3 CellSize2D { get; } = new Vector3(0.5f, 0.866f, 0);

    protected override MazeGenerator Generator => new TriangleMazeGenerator();

    protected override void CreateFinish(MazeGeneratorCell finishCell)
    {
        var finishX = Maze.FinishPosition.X;
        var finishY = Maze.FinishPosition.Y;
        if (finishY == 0)
        {
            var finish = Instantiate(Cell3D,
                new Vector3(Maze.FinishPosition.X * CellSize3D.x, 2f, 
                (Maze.FinishPosition.Y - 1) * CellSize3D.z + DistanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.BottomWall.SetActive(false);
        }
        else if (finishX == finishY)
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.FinishPosition.X - 1) * CellSize3D.x, 2f, 
                Maze.FinishPosition.Y * CellSize3D.z + DistanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.LeftWall.SetActive(false);
        }
        else
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.FinishPosition.X + 1) * CellSize3D.x, 2f, 
                Maze.FinishPosition.Y * CellSize3D.z + DistanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.RightWall.SetActive(false);
        }
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector3(Height / 2f, Height * Mathf.Sqrt(3) / 4f - DistanceBetweenMazes, -2);
        Camera.main.orthographicSize += Width / 2f;
    }

    protected override void SpawnMazeCells(MazeGeneratorCell cell)
    {
        if (cell == null) return;
        if ((cell.X + cell.Y) % 2 != 0) return;
        CreateCell(Cell2D,
            cell,
            new Vector3(cell.X * CellSize2D.x, cell.Y * CellSize2D.y - DistanceBetweenMazes, cell.Y * CellSize2D.z));
        CreateCell(Cell3D,
            cell,
            new Vector3(cell.X * CellSize3D.x, 2 + cell.Y * CellSize3D.y, cell.Y * CellSize3D.z + DistanceBetweenMazes));
    }
}