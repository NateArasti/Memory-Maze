using UnityEngine;

public class TriangleMazeSpawner : MazeSpawner
{
    protected override MazeGenerator Generator => new TriangleMazeGenerator();

    protected override void SetCamera()
    {
        Camera.main.transform.position = new Vector3(Height / 2f, Height * Mathf.Sqrt(3) / 4f - DistanceBetweenMazes, -2);
        Camera.main.orthographicSize = 3f * Height / 5;
    }
}