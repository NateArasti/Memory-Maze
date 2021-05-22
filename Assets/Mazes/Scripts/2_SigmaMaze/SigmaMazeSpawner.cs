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
}
