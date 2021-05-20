using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Classic,
    Arcade
}

public class MazeLoader : MonoBehaviour
{
    private static void LoadMazeScene() => SceneManager.LoadScene("Maze");
    private static void LoadMenuScene() => SceneManager.LoadScene("Menu");

    public void Awake() => Time.timeScale = 1;

    public void LoadMaze(int gameMode)
    {
        if (gameMode > 1) throw new ArgumentException();
        if(gameMode == (int)GameMode.Arcade) ArcadeProgression.MoveToNextProgressionLevel();
        LoadMazeScene();
    }

    public void EndArcadeMode()
    {
        ArcadeProgression.Dispose();
        LoadMenuScene();
    }
    public void RestartArcadeMode()
    {
        ArcadeProgression.Dispose();
        ArcadeProgression.MoveToNextProgressionLevel();
        LoadMazeScene();
    }
}
