public static class DialogueManager
{
    // unnecessary if we don't use this outside of Dialogue.cs
    public static string[] ParseText(string text)
    {
        return text.Split('\n');
    }
}
