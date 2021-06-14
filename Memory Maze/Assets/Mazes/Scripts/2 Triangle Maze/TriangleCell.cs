using System.Collections.Generic;
using UnityEngine;

public class TriangleCell : Cell
{
#pragma warning disable 649
	[SerializeField] private GameObject bottomWall;
	[SerializeField] private GameObject rightWall;
	[SerializeField] private GameObject leftWall;

	private void Awake()
	{
		GameWalls = new Dictionary<int, GameObject>
		{
			{(int) TriangleMazeCell.PossibleWalls.BottomWall, bottomWall},
			{(int) TriangleMazeCell.PossibleWalls.LeftWall, rightWall},
			{(int) TriangleMazeCell.PossibleWalls.RightWall, leftWall}
		};
	}
}