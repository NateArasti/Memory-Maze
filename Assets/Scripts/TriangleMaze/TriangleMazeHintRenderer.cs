using System.Collections.Generic;
using UnityEngine;

public class TriangleMazeHintRenderer : MonoBehaviour
{
    public TriangleMazeSpawner MazeSpawner;

    private LineRenderer LineRenderer;

    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        transform.position = new Vector2(0.5f, 0.4f - MazeSpawner.distanceBetweenMazes);
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

            positions.Add(new Vector2(X / 2f, Y * 0.86f));

            var currentCell = cells[currentPosition.x, currentPosition.y];

            if (X > 0 &&
                !currentCell.LeftWall && cells[X - 1, Y].X != -1 &&
                cells[X - 1, Y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x -= 1;
            }
            else if (Y > 0 &&
                !currentCell.BottomWall && (X + Y) % 2 == 0 &&
                cells[X, Y - 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.y -= 1;
            }
            else if (X < cells.GetLength(0) - 1 &&
                !cells[X, Y].RightWall && cells[X + 1, Y].X != -1 &&
                cells[X + 1, Y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x += 1;
            }
            else if (Y < cells.GetLength(1) - 1 &&
                !cells[X, Y].BottomWall && (X + Y) % 2 == 1 &&
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
