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
	private static GameMode _mode;
	[SerializeField] private GameObject loadingScreen;

	public static bool IsTutorial { get; set; }

	public void Awake()
	{
		Time.timeScale = 1;
	}

	private void LoadMazeScene()
	{
		loadingScreen.SetActive(true);
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
			ChangeScene.SwitchToScene("Maze", "Menu");
		else
			SceneManager.LoadScene("Maze");
	}

	private void LoadMenuScene()
	{
		//SceneManager.LoadScene("Menu");
		ChangeScene.SwitchToScene("Menu", "no Menu lol");
	}

	public void SetGameMode(int newMode)
	{
		_mode = (GameMode) newMode;
	}

	public void SetDifficulty(int newDifficulty)
	{
		ArcadeProgression.CurrentDifficulty = (Difficulty) newDifficulty;
	}

	public void LoadMaze()
	{
		switch (_mode)
		{
			case GameMode.Arcade:
				ArcadeProgression.MoveToNextProgressionLevel();
				LoadMazeScene();
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
		if (ArcadeProgression.ProgressionOn) ArcadeProgression.Dispose();
		if (IsTutorial) TurnOffTutorial();
		LoadMenuScene();
	}

	public void RestartArcadeMode()
	{
		ArcadeProgression.Dispose();
		ArcadeProgression.MoveToNextProgressionLevel();
		LoadMazeScene();
	}

	public void TurnOnTutorial()
	{
		IsTutorial = true;
	}

	public void TurnOffTutorial()
	{
		IsTutorial = false;
	}

	public void SetTutorialMazeCharacteristics()
	{
		MazeCharacteristics.SetMazeCharacteristics(new MazeData(MazeType.PerfectMaze, new[] {5, 5}));
	}
}