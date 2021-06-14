using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour
{
	protected Dictionary<int, GameObject> GameWalls;

	public void SetWalls(Dictionary<int, bool> walls)
	{
		foreach (var wall in walls)
			GameWalls[wall.Key].SetActive(wall.Value);
	}

	public Transform GetWallTransform(int key)
	{
		return GameWalls[key].transform;
	}

	public IEnumerable<Transform> GetWallsTransforms()
	{
		return GameWalls.Select(go => go.Value.transform);
	}
}