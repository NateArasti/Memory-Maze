using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Classic,
    Arcade
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class MazeLoader : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject loadingScreen;
#pragma warning restore 649

    private void LoadMazeScene() => SceneManager.LoadScene("Maze"); //ChangeScene.SwitchToScene("Maze");

    private void LoadMenuScene() => SceneManager.LoadScene("Menu"); //ChangeScene.SwitchToScene("Menu");

    private static GameMode _mode;
    private static Difficulty _difficulty;
    
    public void Awake() => Time.timeScale = 1;
    
    public void SetGameMode(int newMode)
    {
        _mode = (GameMode)newMode;
    }
    
    public void SetDifficulty(int newDifficulty)
    {
        _difficulty = (Difficulty)newDifficulty;
    }

    public void LoadMaze()
    {
        switch (_mode)
        {
            case GameMode.Arcade:
                RestartArcadeMode(_difficulty);
                break;
            case GameMode.Classic:
                LoadMazeScene();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ExitMaze()
    {
        if(ArcadeProgression.ProgressionOn) ArcadeProgression.Dispose();
        LoadMenuScene();
    }

    private void RestartArcadeMode(Difficulty difficulty)
    {
        //TODO ADD DIFFICULTY!
        ArcadeProgression.Dispose();
        ArcadeProgression.MoveToNextProgressionLevel();
        LoadMazeScene();
    }
}
