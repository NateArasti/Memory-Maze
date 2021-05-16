using UnityEngine;

public class SigmaMazeSpawner : MazeSpawner
{
    protected override MazeGenerator Generator => new SigmaMazeGenerator();

    protected override void SetCamera()
    {
        Camera.main.transform.position =
            new Vector3((Width - 1) * 0.87f, 1.5f * (Height - 1) / 2 - DistanceBetweenMazes, -10);
        Camera.main.orthographicSize = Width + 1.5f;
    }

    protected override void CreateFinish(MazeCell finishCell)
    {
        const int bottomRight = (int)SigmaMazeCell.PossibleWalls.BottomRightWall;
        const int bottomLeft = (int)SigmaMazeCell.PossibleWalls.BottomLeftWall;
        const int upperRight = (int)SigmaMazeCell.PossibleWalls.UpperRightWall;
        const int upperLeft = (int)SigmaMazeCell.PossibleWalls.UpperLeftWall;
        const int left = (int)SigmaMazeCell.PossibleWalls.LeftWall;
        const int right = (int)SigmaMazeCell.PossibleWalls.RightWall;
        var neighbors = finishCell.Neighbors;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.Value == null)
            {
                switch (neighbor.Key)
                {
                    case bottomRight:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(0.7f * FinishPositionDelta, 0, -FinishPositionDelta),
                                Quaternion.Euler(0, 150, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case bottomLeft:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(-0.7f * FinishPositionDelta, 0, -FinishPositionDelta),
                                Quaternion.Euler(0, -150, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case upperRight:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(0.7f * FinishPositionDelta, 0, FinishPositionDelta),
                                Quaternion.Euler(0, 30, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case upperLeft:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(-0.7f * FinishPositionDelta, 0, FinishPositionDelta),
                                Quaternion.Euler(0, -30, 0))
                            .transform.localScale = FinishScale;
                        break;
                    case left:
                        Instantiate(
                                Finish,
                                finishCell.Cell3DPosition + new Vector3(FinishPositionDelta, 0, 0),
                                Quaternion.Euler(0, -90, 0))
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
                return;
            }
        }

    }
}
