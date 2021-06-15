using System.Collections.Generic;
using UnityEngine;

public class PerfectCell : Cell
{
	[SerializeField] private GameObject bottomWall;
	[SerializeField] private GameObject rightWall;
	[SerializeField] private GameObject leftWall;
	[SerializeField] private GameObject upperWall;

	private void Awake()
	{
		const int bottom = (int) PerfectMazeCell.PossibleWalls.BottomWall;
		const int right = (int) PerfectMazeCell.PossibleWalls.RightWall;
		const int left = (int) PerfectMazeCell.PossibleWalls.LeftWall;
		const int upper = (int) PerfectMazeCell.PossibleWalls.UpperWall;

		GameWalls = new Dictionary<int, GameObject>
		{
			{bottom, bottomWall},
			{right, rightWall},
			{left, leftWall},
			{upper, upperWall}
		};
	}
}