using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    protected Dictionary<int, GameObject> GameWalls;
    public void SetWalls(Dictionary<int, bool> walls)
    {
        foreach (var wall in walls) 
            GameWalls[wall.Key].SetActive(wall.Value);
    }
}
