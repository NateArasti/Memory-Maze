using System;
using System.Collections.Generic;
using UnityEngine;

public enum MazeType
{
    PerfectMaze,
    TriangleMaze,
    SigmaMaze,
}

public class MazeCharacteristics : MonoBehaviour
{
    public static MazeType CurrentMazeType { get; private set; } = MazeType.PerfectMaze;

    public static IReadOnlyDictionary<MazeType, (string[] paramNames, int[] paramValues, Vector2 valueRange)>
        Characteristics =
            new Dictionary<MazeType, (string[] paramNames, int[] paramValues, Vector2 valueRange)>
            {
                [MazeType.PerfectMaze] = (
                    paramNames: new[] {"PerfectMazeWidth", "PerfectMazeHeight"},
                    paramValues: new[] {5, 5},
                    new Vector2(5, 40)),
                [MazeType.TriangleMaze] = (
                    paramNames: new[] {"TriangleMazeSide"},
                    paramValues: new[] {5},
                    new Vector2(5, 40)),
                [MazeType.SigmaMaze] = (
                    paramNames: new[] {"SigmaMazeSide"},
                    paramValues: new[] {3},
                    new Vector2(3, 20)),
            };

    private void Awake()
    {
        foreach (var mazeType in Characteristics)
            for (var i = 0; i < Characteristics[mazeType.Key].paramNames.Length; i++)
            {
                Characteristics[mazeType.Key].paramValues[i] =
                    PlayerPrefs.GetInt(Characteristics[mazeType.Key].paramNames[i],
                        Characteristics[mazeType.Key].paramValues[i]);
            }
    }

    public static void SetMazeCharacteristics(MazeData data)
    {
        if (data.Params.Length != Characteristics[data.MazeType].paramValues.Length)
            throw new ArgumentException("ParamValues are unsuitable for this maze type");
        CurrentMazeType = data.MazeType;
        for (var i = 0; i < Characteristics[CurrentMazeType].paramNames.Length; i++)
        {
            Characteristics[CurrentMazeType].paramValues[i] = data.Params[i];
        }
    }

    public static void SaveMazesCharacteristics()
    {
        foreach (var mazeType in Characteristics)
            for (var i = 0; i < Characteristics[mazeType.Key].paramNames.Length; i++)
            {
                PlayerPrefs.SetInt(Characteristics[mazeType.Key].paramNames[i], 
                    Characteristics[mazeType.Key].paramValues[i]);
            }
        PlayerPrefs.Save();
    }
}