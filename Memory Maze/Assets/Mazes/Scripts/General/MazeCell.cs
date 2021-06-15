using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeCell
{
	protected Vector3 Cell2DSize;
	protected Vector3 Cell3DSize;

	public Dictionary<int, bool> Walls { get; protected set; }
	public Dictionary<int, MazeCell> Neighbors { get; protected set; }
	public bool Visited { get; set; }
	public int DistanceFromStart { get; private set; }

	public Vector3 Cell2DPosition { get; private set; }
	public Vector3 Cell3DPosition { get; private set; }

	protected int DistanceBetweenMazes => 50;

	public void SetDistance(MazeCell cell)
	{
		DistanceFromStart = cell.DistanceFromStart + 1;
	}

	public void SetPositions(int x, int y)
	{
		Cell2DPosition = new Vector3(x * Cell2DSize.x, y * Cell2DSize.y - DistanceBetweenMazes, y * Cell2DSize.z);
		Cell3DPosition = new Vector3(x * Cell3DSize.x, y * Cell3DSize.y + DistanceBetweenMazes, y * Cell3DSize.z);
	}

	public static List<MazeCell> GetNeighborsList(MazeCell currentCell, bool check) => 
        currentCell.Neighbors.Values.Where(cell => cell != null && cell.Visited == check).ToList();
}