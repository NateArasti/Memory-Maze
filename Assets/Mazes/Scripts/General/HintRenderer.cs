using System.Collections.Generic;
using UnityEngine;

public class HintRenderer : MonoBehaviour
{
    [SerializeField] private MazeSpawner MazeSpawner;
    [SerializeField] private Vector2 scaleVector;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawPath()
    {
        var cells = MazeSpawner.Maze.Cells;
        var currentPosition = new Vector2Int(MazeSpawner.Maze.FinishPosition.X, MazeSpawner.Maze.FinishPosition.Y);
        var startPosition = new Vector2Int(MazeSpawner.Maze.StartPosition.X, MazeSpawner.Maze.StartPosition.Y);
        var positions = new List<Vector3>();

        while (currentPosition != startPosition)
        {
            var x = currentPosition.x;
            var y = currentPosition.y;

            positions.Add(new Vector2(x, y) * scaleVector);

            var currentCell = cells[currentPosition.x, currentPosition.y];

            if (x > 0 &&
                !currentCell.LeftWall && cells[x - 1, y] != null &&
                cells[x - 1, y].DistanceFromStart == currentCell.DistanceFromStart - 1)
                currentPosition.x -= 1;
            else if (y > 0 &&
                     !currentCell.BottomWall &&
                     cells[x, y - 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
                currentPosition.y -= 1;
            else if (x < MazeSpawner.Width - 1 &&
                     !cells[x, y].RightWall && cells[x + 1, y] != null &&
                     cells[x + 1, y].DistanceFromStart == currentCell.DistanceFromStart - 1)
                currentPosition.x += 1;
            else if (y < MazeSpawner.Height - 1 &&
                     !cells[x, y].UpperWall &&
                     cells[x, y + 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
                currentPosition.y += 1;
        }

        positions.Add((Vector2) startPosition);
        _lineRenderer.positionCount = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }
}