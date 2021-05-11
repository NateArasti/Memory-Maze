using System.Collections.Generic;
using UnityEngine;

public class SigmaCell : Cell
{
#pragma warning disable 649
    [SerializeField] private GameObject bottomRightWall;
    [SerializeField] private GameObject bottomLeftWall;
    [SerializeField] private GameObject upperRightWall;
    [SerializeField] private GameObject upperLeftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;

    private void Awake()
    {
        var bottomRight = (int)SigmaMazeCell.PossibleWalls.BottomRightWall;
        var bottomLeft = (int)SigmaMazeCell.PossibleWalls.BottomLeftWall;
        var upperRight = (int)SigmaMazeCell.PossibleWalls.UpperRightWall;
        var upperLeft = (int)SigmaMazeCell.PossibleWalls.UpperLeftWall;
        var left = (int)SigmaMazeCell.PossibleWalls.LeftWall;
        var right = (int)SigmaMazeCell.PossibleWalls.RightWall;

        GameWalls = new Dictionary<int, GameObject>
        {
            {bottomRight, bottomRightWall},
            {bottomLeft, bottomLeftWall},
            {upperRight, upperRightWall},
            {upperLeft, upperLeftWall},
            {left, leftWall},
            {right, rightWall}
        };
    }
}
