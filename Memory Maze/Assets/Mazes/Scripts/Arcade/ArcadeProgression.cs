using System.Collections.Generic;

public class ArcadeProgression
{
    public static bool ProgressionOn { get; private set; }
    public static Difficulty CurrentDifficulty { get; set; }

    public static int ProgressLevelsCount => ProgressDataSets[CurrentDifficulty].Count;//15?

    // Lengths ranges in MazeCharacteristics.cs
    private static readonly IReadOnlyList<MazeData> EasyProgressData = new[]
    {
        new MazeData(MazeType.TriangleMaze, new[] {5}),
        new MazeData(MazeType.PerfectMaze, new[] {5, 5}),
        new MazeData(MazeType.TriangleMaze, new[] {7}),
        new MazeData(MazeType.PerfectMaze, new[] {7, 6}),
        new MazeData(MazeType.SigmaMaze, new[] {3}),
        new MazeData(MazeType.TriangleMaze, new[] {9}),
        new MazeData(MazeType.PerfectMaze, new[] {9, 8}),
        new MazeData(MazeType.TriangleMaze, new[] {10}),
        new MazeData(MazeType.PerfectMaze, new[] {12, 10}),
        new MazeData(MazeType.SigmaMaze, new[] {4}),
        new MazeData(MazeType.PerfectMaze, new[] {13, 13}),
        new MazeData(MazeType.TriangleMaze, new[] {15}),
        new MazeData(MazeType.PerfectMaze, new[] {15, 17}),
        new MazeData(MazeType.SigmaMaze, new[] {5}),
        new MazeData(MazeType.TriangleMaze, new[] {15}),
    };
    private static readonly IReadOnlyList<MazeData> NormalProgressData = new[]
    {
        new MazeData(MazeType.PerfectMaze, new[] {17, 17}),
        new MazeData(MazeType.SigmaMaze, new[] {6}),
        new MazeData(MazeType.TriangleMaze, new[] {17}),
        new MazeData(MazeType.PerfectMaze, new[] {18, 19}),
        new MazeData(MazeType.SigmaMaze, new[] {7}),
        new MazeData(MazeType.TriangleMaze, new[] {20}),
        new MazeData(MazeType.PerfectMaze, new[] {20, 20}),
        new MazeData(MazeType.SigmaMaze, new[] {8}),
        new MazeData(MazeType.TriangleMaze, new[] {22}),
        new MazeData(MazeType.PerfectMaze, new[] {22, 18}),
        new MazeData(MazeType.TriangleMaze, new[] {25}),
        new MazeData(MazeType.SigmaMaze, new[] {9}),
        new MazeData(MazeType.TriangleMaze, new[] {27}),
        new MazeData(MazeType.SigmaMaze, new[] {10}),
        new MazeData(MazeType.PerfectMaze, new[] {25, 25 }),
    };
    private static readonly IReadOnlyList<MazeData> HardProgressData = new[]
    {
        new MazeData(MazeType.SigmaMaze, new[] {11}),
        new MazeData(MazeType.TriangleMaze, new[] {28}),
        new MazeData(MazeType.PerfectMaze, new[] {25, 25}),
        new MazeData(MazeType.SigmaMaze, new[] {12}),
        new MazeData(MazeType.PerfectMaze, new[] {28, 26}),
        new MazeData(MazeType.TriangleMaze, new[] {31}),
        new MazeData(MazeType.SigmaMaze, new[] {13}),
        new MazeData(MazeType.PerfectMaze, new[] {30, 27}),
        new MazeData(MazeType.TriangleMaze, new[] {35}),
        new MazeData(MazeType.PerfectMaze, new[] {33, 32}),
        new MazeData(MazeType.TriangleMaze, new[] {38}),
        new MazeData(MazeType.SigmaMaze, new[] {14}),
        new MazeData(MazeType.TriangleMaze, new[] {40}),
        new MazeData(MazeType.PerfectMaze, new[] {35, 35}),
        new MazeData(MazeType.SigmaMaze, new[] {15}),
    };

    private static readonly IReadOnlyDictionary<Difficulty, IReadOnlyList<MazeData>> ProgressDataSets =
        new Dictionary<Difficulty, IReadOnlyList<MazeData>>
        {
            [Difficulty.Easy] = EasyProgressData,
            [Difficulty.Normal] = NormalProgressData,
            [Difficulty.Hard] = HardProgressData,
        };

public static int CurrentIndex { get; private set; }

    public static void MoveToNextProgressionLevel()
    {
        ProgressionOn = true;
        MazeCharacteristics.SetMazeCharacteristics(ProgressDataSets[CurrentDifficulty][CurrentIndex]);
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
