namespace SunamoCSharp._sunamo;

/// <summary>
/// Helpers for working with strings.
/// </summary>
internal class SH
{

    /// <summary>
    /// Wraps the value(s) with the given prefix and suffix.
    /// </summary>
    internal static string WrapWith(string value, string wrapper)
    {
        return wrapper + value + wrapper;
    }

    /// <summary>
    /// Wraps the value with quotation marks.
    /// </summary>
    internal static string WrapWithQm(string value)
    {
        var wrapper = "\"";
        return wrapper + value + wrapper;
    }

    /// <summary>
    /// Wraps the value with backslashes.
    /// </summary>
    internal static string WrapWithBs(string value)
    {
        var wrapper = "\\";
        return wrapper + value + wrapper;
    }
}
