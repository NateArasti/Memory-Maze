using System.Collections.Generic;
using UnityEngine;

public class PerfectMazeCell : MazeCell
{
	public enum PossibleWalls
	{
		BottomWall,
		LeftWall,
		UpperWall,
		RightWall
	}

	public PerfectMazeCell()
	{
		Cell2DSize = new Vector3(1, 1, 0);
		Cell3DSize = new Vector3(10, 0, 10);
		var bottom = (int) PossibleWalls.BottomWall;
		var left = (int) PossibleWalls.LeftWall;
		var upper = (int) PossibleWalls.UpperWall;
		var right = (int) PossibleWalls.RightWall;

		Walls = new Dictionary<int, bool>
		{
			{bottom, true},
			{left, true},
			{upper, true},
			{right, true}
		};

		Neighbors = new Dictionary<int, MazeCell>
		{
			{bottom, null},
			{left, null},
			{upper, null},
			{right, null}
		};
	}
}