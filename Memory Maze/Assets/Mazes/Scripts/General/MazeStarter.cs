using UnityEngine;

public class MazeStarter : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] mazeSpawners;
    [SerializeField] private Material simpleSkyBox;
    [SerializeField] private Material redSkyBox;

    public Maze CurrentMaze { get; private set; }

    private void Awake()
    {
        RenderSettings.skybox = Random.value > 0.98f ? redSkyBox : simpleSkyBox;
        CurrentMaze = Instantiate(mazeSpawners[(int)MazeCharacteristics.CurrentMazeType])
            .GetComponent<MazeSpawner>().Maze;
    }
}