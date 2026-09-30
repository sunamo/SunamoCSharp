namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

/// <summary>
/// Splitting of strings.
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits the text by the delimiters.
    /// </summary>
    internal static List<string> Split(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }




    /// <summary>
    /// Splits the text by white spaces.
    /// </summary>
    internal static List<string> SplitByWhiteSpaces(string text)
    {
        WhitespaceCharService whitespaceChar = new();
        return text.Split(whitespaceChar.WhiteSpaceChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}