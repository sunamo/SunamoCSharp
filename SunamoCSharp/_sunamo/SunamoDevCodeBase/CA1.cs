namespace SunamoCSharp._sunamo;

/// <summary>
/// Helpers for working with collections.
/// </summary>
internal partial class CA
{

    /// <summary>
    /// Trims the given characters or suffix from the end.
    /// </summary>
    internal static List<string> TrimEnd(List<string> list, params char[] toTrim)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].TrimEnd(toTrim);
        }

        return list;
    }

    /// <summary>
    /// Removes empty lines from the beginning up to the first non-empty line.
    /// </summary>
    internal static void RemoveEmptyLinesToFirstNonEmpty(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Trim() == string.Empty)
            {
                lines.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Removes the lines with the given indexes.
    /// </summary>
    internal static void RemoveLines(List<string> lines, List<int> lineIndexesToRemove)
    {
        lineIndexesToRemove.Sort();
        for (int i = lineIndexesToRemove.Count - 1; i >= 0; i--)
        {
            var lineIndex = lineIndexesToRemove[i];
            lines.RemoveAt(lineIndex);
        }
    }

    /// <summary>
    /// Removes empty and whitespace-only strings from the list.
    /// </summary>
    internal static List<string> RemoveStringsEmpty2(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == string.Empty)
            {
                list.RemoveAt(i);
            }
        }

        return list;
    }
}
