using System.Collections.Generic;

public class ArcadeProgression
{
    public static bool ProgressionOn { get; private set; }

    // Lengths ranges in MazeCharacteristics.cs
    public static IReadOnlyList<MazeData> ProgressData = new[]
    {
        new MazeData(MazeType.TriangleMaze, new[] {5}),
        new MazeData(MazeType.PerfectMaze, new[] {5, 5}),
        new MazeData(MazeType.TriangleMaze, new[] {7}),
        new MazeData(MazeType.PerfectMaze, new[] {6, 6}),
        new MazeData(MazeType.SigmaMaze, new[] {3}),
        new MazeData(MazeType.TriangleMaze, new[] {8}),
        new MazeData(MazeType.PerfectMaze, new[] {10, 6}),
        new MazeData(MazeType.TriangleMaze, new[] {10}),
        new MazeData(MazeType.SigmaMaze, new[] {4}),
        new MazeData(MazeType.TriangleMaze, new[] {12}),
        new MazeData(MazeType.SigmaMaze, new[] {5}),
        new MazeData(MazeType.PerfectMaze, new[] {10, 10}),
        new MazeData(MazeType.TriangleMaze, new[] {15}),
        new MazeData(MazeType.PerfectMaze, new[] {13, 15}),
        new MazeData(MazeType.TriangleMaze, new[] {18}),
    };

    public static int CurrentIndex { get; private set; }

    public static void MoveToNextProgressionLevel()
    {
        ProgressionOn = true;
        MazeCharacteristics.SetMazeCharacteristics(ProgressData[CurrentIndex]);
        CurrentIndex += 1;
    }

    public static void Dispose()
    {
        CurrentIndex = 0;
        ProgressionOn = false;
    }
}

public readonly struct MazeData
{
    public readonly MazeType MazeType;
    public readonly int[] Params;

    public MazeData(MazeType mazeType, int[] @params)
    {
        MazeType = mazeType;
        Params = @params;
    }
}
