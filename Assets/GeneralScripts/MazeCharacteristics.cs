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
    public static MazeType CurrentMazeType { get; private set; } = MazeType.TriangleMaze;

    public static IReadOnlyDictionary<MazeType, (string[] paramNames, int[] paramValues)> Characteristics =
        new Dictionary<MazeType, (string[] paramNames, int[] paramValues)>
        {
            [MazeType.PerfectMaze] = (
                paramNames: new[] { "PerfectMazeWidth", "PerfectMazeHeight" },
                paramValues: new[] { 5, 5 }),
            [MazeType.TriangleMaze] = (
                paramNames: new[] { "TriangleMazeSide" },
                paramValues: new[] { 5 }),
            [MazeType.SigmaMaze] = (
                paramNames: new[] { "SigmaMazeSide" },
                paramValues: new[] { 5 }),
        };

    private void Awake()
    {
        foreach (var mazeType in Characteristics)
            for (var i = 0; i < Characteristics[mazeType.Key].paramNames.Length; i++)
                Characteristics[mazeType.Key].paramValues[i] =
                    PlayerPrefs.GetInt(Characteristics[mazeType.Key].paramNames[i]);
    }

    public void SetMazeCharacteristics(MazeType newMazeType, params int[] paramValues)
    {
        if (paramValues.Length != Characteristics[CurrentMazeType].paramValues.Length)
            throw new ArgumentException("ParamValues are unsuitable for this maze type");
        CurrentMazeType = newMazeType;
        for (var i = 0; i < Characteristics[CurrentMazeType].paramNames.Length; i++)
        {
            PlayerPrefs.SetInt(Characteristics[CurrentMazeType].paramNames[i], paramValues[i]);
            Characteristics[CurrentMazeType].paramValues[i] = paramValues[i];
        }
        PlayerPrefs.Save();
    }
}