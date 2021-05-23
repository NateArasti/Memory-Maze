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
    private static void LoadMazeScene() => ChangeScene.SwitchToScene("Maze");
    private static void LoadMenuScene() => ChangeScene.SwitchToScene("Menu");

    private GameMode mode;
    private Difficulty difficulty;
    
    public void Awake() => Time.timeScale = 1;
    
    public void SetGameMode(int newMode)
    {
        mode = (GameMode)newMode;
    }
    
    public void SetDifficulty(int newDifficulty)
    {
        difficulty = (Difficulty)newDifficulty;
    }

    public void LoadMaze()
    {
        switch (mode)
        {
            case GameMode.Arcade: 
                RestartArcadeMode(difficulty);
                break;
            case GameMode.Classic:
                LoadMazeScene();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    public void EndArcadeMode()
    {
        ArcadeProgression.Dispose();
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
