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
        var bottom = (int)TriangleMazeCell.PossibleWalls.BottomWall;
        var left = (int)TriangleMazeCell.PossibleWalls.LeftWall;
        var right = (int)TriangleMazeCell.PossibleWalls.RightWall;
        GameWalls = new Dictionary<int, GameObject>
        {
            {bottom, bottomWall},
            {right, rightWall},
            {left, leftWall}
        };
    }
}
