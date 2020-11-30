using System.Collections.Generic;
using UnityEngine;

public class PerfectMazeHintRenderer : MonoBehaviour
{
    public PerfectMazeSpawner MazeSpawner;

    private LineRenderer LineRenderer;

    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        transform.position = new Vector3(0.7f, 0.8f - MazeSpawner.distanceBetweenMazes, 3.5f);
    }

    public void DrawPath()
    {
        var cells = MazeSpawner.Maze.cells;
        var currentPosition = new Vector2Int(MazeSpawner.Maze.finishPosition.X, MazeSpawner.Maze.finishPosition.Y);
        var startPosition = new Vector2Int(MazeSpawner.Maze.startPosition.X, MazeSpawner.Maze.startPosition.Y);
        var positions = new List<Vector3>();

        while (currentPosition != startPosition)
        {
            var X = currentPosition.x;
            var Y = currentPosition.y;

            positions.Add(new Vector2(X, Y));

            var currentCell = cells[currentPosition.x, currentPosition.y];

            if (X > 0 &&
                !currentCell.LeftWall &&
                cells[X - 1, Y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x -= 1;
            }
            else if (Y > 0 &&
                !currentCell.BottomWall &&
                cells[X, Y - 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.y -= 1;
            }
            else if (X < cells.GetLength(0) - 1 &&
                !cells[X, Y].RightWall &&
                cells[X + 1, Y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x += 1;
            }
            else if (Y < cells.GetLength(1) - 1 &&
                !cells[X, Y].UpperWall &&
                cells[X, Y + 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.y += 1;
            }
        }

        positions.Add((Vector2)startPosition);
        LineRenderer.positionCount = positions.Count;
        LineRenderer.SetPositions(positions.ToArray());
    }
}
