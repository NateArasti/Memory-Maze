using System.Collections.Generic;
using UnityEngine;

public class PerfectCell : Cell
{
#pragma warning disable 649
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject upperWall;

    private void Awake()
    {
        var bottom = (int)PerfectMazeCell.PossibleWalls.BottomWall;
        var right = (int)PerfectMazeCell.PossibleWalls.RightWall;
        var left = (int)PerfectMazeCell.PossibleWalls.LeftWall;
        var upper = (int)PerfectMazeCell.PossibleWalls.UpperWall;

        GameWalls = new Dictionary<int, GameObject>
        {
            {bottom, bottomWall},
            {right, rightWall},
            {left, leftWall},
            {upper, upperWall}
        };
    }
}
