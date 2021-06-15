using UnityEngine;

public class PerfectMazeSpawner : MazeSpawner
{
	protected override MazeGenerator Generator => new PerfectMazeGenerator();

	protected override void SetCamera()
	{
		Camera.main!.transform.position = new Vector3(Width / 2f, Height / 2f - DistanceBetweenMazes);
		Camera.main.orthographicSize = Mathf.Max(Width / 3f, 2f * Height / 3);
	}
}