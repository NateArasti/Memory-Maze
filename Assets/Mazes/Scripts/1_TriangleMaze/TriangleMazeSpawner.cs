using UnityEngine;

public class TriangleMazeSpawner : MazeSpawner
{
    protected override MazeGenerator Generator => new TriangleMazeGenerator();

    protected override void CreateFinish(MazeCell finishCell)
    {
        const int bottom = (int)TriangleMazeCell.PossibleWalls.BottomWall;
        const int left = (int)TriangleMazeCell.PossibleWalls.LeftWall;
        const int right = (int)TriangleMazeCell.PossibleWalls.RightWall;
        var neighbors = finishCell.Neighbors;
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Value == null)
            {
                switch (neighbor.Key)
                {
                    case bottom:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(0, 0, -FinishPositionDelta),
                                Quaternion.Euler(0, 180, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case left:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(-0.7f * FinishPositionDelta, 0, 0),
                                Quaternion.Euler(0, -60, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case right:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(0.7f * FinishPositionDelta, 0, 0),
                                Quaternion.Euler(0, 60, 0))
                            .transform.localScale = FinishScale;
                        break;
                }
            }
        }
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector3(Height / 2f, Height * Mathf.Sqrt(3) / 4f - DistanceBetweenMazes, -2);
        Camera.main.orthographicSize = Height;
    }
}