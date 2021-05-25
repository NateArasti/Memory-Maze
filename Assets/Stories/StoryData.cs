public struct StoryData
{
    public readonly string StoryOnRussian;
    public readonly string StoryOnEnglish;
    public bool IsCollected;
    public readonly int Index;

    public StoryData(string storyOnRussian, string storyOnEnglish, int index)
    {
        StoryOnRussian = storyOnRussian;
        StoryOnEnglish = storyOnEnglish;
        Index = index;
        IsCollected = false;
    }
}