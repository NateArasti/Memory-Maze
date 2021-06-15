using System.Collections.Generic;
using UnityEngine;

public class TriangleMazeCell : MazeCell
{
	public enum PossibleWalls
	{
		BottomWall,
		LeftWall,
		RightWall
	}

	public bool IsUpsideDown;

	public TriangleMazeCell()
	{
		Cell2DSize = new Vector3(0.5f, 0.866f, 0);
		Cell3DSize = new Vector3(6, 0, 10.3923f);
		var bottom = (int) PossibleWalls.BottomWall;
		var left = (int) PossibleWalls.LeftWall;
		var right = (int) PossibleWalls.RightWall;

		Walls = new Dictionary<int, bool>
		{
			{bottom, true},
			{left, true},
			{right, true}
		};

		Neighbors = new Dictionary<int, MazeCell>
		{
			{bottom, null},
			{left, null},
			{right, null}
		};
	}
}