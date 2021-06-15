using System.Collections.Generic;
using UnityEngine;

public class SigmaMazeCell : MazeCell
{
	public enum PossibleWalls
	{
		BottomRightWall,
		BottomLeftWall,
		UpperRightWall,
		UpperLeftWall,
		LeftWall,
		RightWall
	}

	public SigmaMazeCell()
	{
		Cell2DSize = new Vector3(0.87f, 1.5f, 0);
		Cell3DSize = new Vector3(6.108f, 0, 10.58f);
		var bottomRight = (int) PossibleWalls.BottomRightWall;
		var bottomLeft = (int) PossibleWalls.BottomLeftWall;
		var upperRight = (int) PossibleWalls.UpperRightWall;
		var upperLeft = (int) PossibleWalls.UpperLeftWall;
		var left = (int) PossibleWalls.LeftWall;
		var right = (int) PossibleWalls.RightWall;

		Walls = new Dictionary<int, bool>
		{
			{bottomRight, true},
			{bottomLeft, true},
			{upperRight, true},
			{upperLeft, true},
			{left, true},
			{right, true}
		};

		Neighbors = new Dictionary<int, MazeCell>
		{
			{bottomRight, null},
			{bottomLeft, null},
			{upperRight, null},
			{upperLeft, null},
			{left, null},
			{right, null}
		};
	}
}