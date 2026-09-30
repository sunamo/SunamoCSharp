namespace SunamoCSharp._sunamo;

/// <summary>
/// Replacing in strings.
/// </summary>
internal class SHReplace
{

    /// <summary>
    /// Replaces only the first occurrence of the pattern.
    /// </summary>
    internal static string ReplaceOnce(string input, string pattern, string replacement)
    {
        return new Regex(pattern).Replace(input, replacement, 1);
    }
}
