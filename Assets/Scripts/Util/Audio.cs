/// <summary>
/// Stores Default audio BGM
/// </summary>
public static class Audio
{
    /// <summary>
    /// Win music
    /// </summary>
    public static BGM bossa { get; private set; }

    // add sfx and other default audio if applicable...

    static Audio()
    {
        bossa = new BGM($"8bossa");
    }
}