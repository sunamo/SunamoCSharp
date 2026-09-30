namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

/// <summary>
/// Helpers for working with collections.
/// </summary>
internal partial class CA
{

    /// <summary>
    /// Removes null, empty and whitespace-only elements from the list.
    /// </summary>
    internal static void RemoveNullEmptyWs(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrWhiteSpace(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Trims every element of the list.
    /// </summary>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = list[i].Trim();
        return list;
    }

    /// <summary>
    /// Replaces repeated empty lines in the text with a single one.
    /// </summary>
    internal static void DoubleOrMoreMultiLinesToSingle(ref string text)
    {
        text = Regex.Replace(text, @"(\r?\n\s*){2,}", Environment.NewLine + Environment.NewLine);
        text = text.Trim();
    }

    /// <summary>
    /// Replaces whitespace-only elements with an empty string.
    /// </summary>
    internal static void TrimWhereIsOnlyWhitespace(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (string.IsNullOrWhiteSpace(item))
            {
                list[i] = list[i].Trim();
            }
        }
    }
}