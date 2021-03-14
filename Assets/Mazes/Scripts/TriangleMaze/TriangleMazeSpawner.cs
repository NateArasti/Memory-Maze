using UnityEngine;

public class TriangleMazeSpawner : MazeSpawner
{
    public override Vector3 cellSize3D { get; } = new Vector3(6, 0, 10.3923f);
    public override Vector3 cellSize2D { get; } = new Vector3(0.5f, 0.866f, 0);

    protected override MazeGenerator generator => new TriangleMazeGenerator();

    protected override void CreateFinish(MazeGeneratorCell finishCell)
    {
        var finishX = Maze.finishPosition.X;
        var finishY = Maze.finishPosition.Y;
        if (finishY == 0)
        {
            var finish = Instantiate(Cell3D,
                new Vector3(Maze.finishPosition.X * cellSize3D.x, 2f, 
                (Maze.finishPosition.Y - 1) * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.BottomWall.SetActive(false);
        }
        else if (finishX == finishY)
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.finishPosition.X - 1) * cellSize3D.x, 2f, 
                Maze.finishPosition.Y * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.LeftWall.SetActive(false);
        }
        else
        {
            var finish = Instantiate(Cell3D,
                new Vector3((Maze.finishPosition.X + 1) * cellSize3D.x, 2f, 
                Maze.finishPosition.Y * cellSize3D.z + distanceBetweenMazes + 0.5f),
                Quaternion.Euler(0, 180, 0)).GetComponent<MazeCell>();
            finish.RightWall.SetActive(false);
        }
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector3(height / 2f, height * Mathf.Sqrt(3) / 4f - distanceBetweenMazes, -2);
        Camera.main.orthographicSize += width / 2f;
    }

    protected override void SpawnMazeCells(MazeGeneratorCell cell)
    {
        if (cell != null)
        {
            if ((cell.X + cell.Y) % 2 == 0)
            {
                CreateCell(Cell2D,
                    cell,
                    new Vector3(cell.X * cellSize2D.x, cell.Y * cellSize2D.y - distanceBetweenMazes, cell.Y * cellSize2D.z));
                CreateCell(Cell3D,
                    cell,
                    new Vector3(cell.X * cellSize3D.x, 2 + cell.Y * cellSize3D.y, cell.Y * cellSize3D.z + distanceBetweenMazes));
            }
        }
    }
}