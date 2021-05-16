using UnityEngine;

public class PerfectMazeSpawner : MazeSpawner
{
    protected override MazeGenerator Generator => new PerfectMazeGenerator();

    protected override void CreateFinish(MazeCell finishCell)
    {
        const int bottom = (int)PerfectMazeCell.PossibleWalls.BottomWall;
        const int left = (int)PerfectMazeCell.PossibleWalls.LeftWall;
        const int upper = (int)PerfectMazeCell.PossibleWalls.UpperWall;
        const int right = (int)PerfectMazeCell.PossibleWalls.RightWall;
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
                                finishCell.Cell3DPosition + new Vector3(-FinishPositionDelta, 0, 0),
                                Quaternion.Euler(0, -90, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case upper:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(0, 0, FinishPositionDelta),
                                Quaternion.Euler(0, 0, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case right:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(FinishPositionDelta, 0, 0),
                                Quaternion.Euler(0, 90, 0))
                            .transform.localScale = FinishScale;
                        break;
                }
            }
        }
    }

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector3(Width / 2f, Height / 2f - DistanceBetweenMazes);
        Camera.main.orthographicSize += Mathf.Max(Height, Width) / 1.5f;
    }
}