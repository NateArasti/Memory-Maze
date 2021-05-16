using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeStarter : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] mazeSpawners;

    public Maze CurrentMaze { get; private set; }

    private void Awake()
    {
        CurrentMaze = Instantiate(mazeSpawners[(int)MazeCharacteristics.CurrentMazeType])
            .GetComponent<MazeSpawner>().Maze;
    }
}