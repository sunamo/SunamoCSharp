namespace SunamoCSharp.Helpers;

/// <summary>
/// Represents the c sharp helper sunamo type.
/// </summary>
public class CSharpHelperSunamo
{
    internal static Type Type = typeof(CSharpHelperSunamo);

    /// <summary>
    /// Is all csproj and sln right in hiearchy.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="textOutputGenerator">The text output generator.</param>
    /// <returns>The is all csproj and sln right in hiearchy result.</returns>
    public static string IsAllCsprojAndSlnRightInHiearchy(string path, TextOutputGeneratorDC textOutputGenerator)
    {
        var csprojFiles = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).ToList();
        var slnFiles = new List<string>();
        foreach (var ext in new[] { "*.sln", "*.slnx", "*.slnj" })
        {
            slnFiles.AddRange(Directory.GetFiles(path, ext, SearchOption.AllDirectories));
        }

        List<string> incorrectSlnFolders = new List<string>();

        for (int i = slnFiles.Count - 1; i >= 0; i--)
        {
            if (!IsOnlyInSpecialOrProjectFolders(slnFiles[i]))
            {
                incorrectSlnFolders.Add(slnFiles[i]);
                slnFiles.RemoveAt(i);
            }
        }

        for (int i = 0; i < slnFiles.Count; i++)
        {
            slnFiles[i] = Path.GetDirectoryName(slnFiles[i])!;
        }

        for (int i = csprojFiles.Count - 1; i >= 0; i--)
        {
            var csprojFolder = Path.GetDirectoryName(csprojFiles[i]);
            var slnFolder = Path.GetDirectoryName(csprojFolder);

            if (slnFiles.Contains(slnFolder!))
            {
                csprojFiles.RemoveAt(i);
            }
        }

        textOutputGenerator.List(incorrectSlnFolders, "Sln in wrong folder:");
        textOutputGenerator.List(csprojFiles, "Csproj in wrong folder:");

        return textOutputGenerator.ToString()!;
    }

    private static bool IsOnlyInSpecialOrProjectFolders(string filePath)
    {
        filePath = Path.GetDirectoryName(filePath)!;

        while (true)
        {
            filePath = Path.GetDirectoryName(filePath)!;

            if (filePath!.Length < 4)
            {
                return false;
            }

            var fileName = Path.GetFileName(filePath);

            if (fileName.EndsWith("Projects"))
            {
                return true;
            }

            if (!fileName.StartsWith("_"))
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Detect from to string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The detect from to string result.</returns>
    public static FromToList DetectFromToString(string text)
    {
        List<int> quoteIndices = null!;// SH.ReturnOccurencesOfString(text, "\"");
        for (int i = quoteIndices.Count - 1; i >= 0; i--)
        {
            if (text[quoteIndices[i] - 1] == '\\')
            {
                quoteIndices.RemoveAt(i);
            }
        }

        ThrowEx.HasOddNumberOfElements("quoteIndices", quoteIndices);

        var fromToList = new FromToList();
        for (int i = 0; i < quoteIndices.Count; i++)
        {
            fromToList.Ranges.Add(new FromToDC(quoteIndices[i], quoteIndices[++i]));
        }
        return fromToList;
    }

    /// <summary>
    /// Indent as previous line.
    /// </summary>
    /// <param name="lines">The lines.</param>
    public static void IndentAsPreviousLine(List<string> lines)
    {
        string previousIndent = string.Empty;
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Length == 0)
            {
                continue;
            }

            if (!char.IsWhiteSpace(line[0]))
            {
                lines[i] = previousIndent + line;
                continue;
            }

            StringBuilder whitespaceBuilder = new StringBuilder();
            bool hasNonWhitespace = false;
            foreach (var item in line)
            {
                if (char.IsWhiteSpace(item))
                {
                    whitespaceBuilder.Append(item);
                }
                else
                {
                    hasNonWhitespace = true;
                    break;
                }
            }

            if (hasNonWhitespace)
            {
                previousIndent = whitespaceBuilder.ToString();
            }
            else
            {
                // Line contains only whitespace - normalize it to a blank line and
                // reset the indent reference, so a following unindented line (e.g. a
                // closing brace) is not wrongly re-indented.
                lines[i] = string.Empty;
                previousIndent = string.Empty;
            }
        }
    }

    /// <summary>
    /// Is interface.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The is interface result.</returns>
    public static bool IsInterface(string text)
    {
        if (text[0] == 'I')
        {
            if (char.IsUpper(text[1]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Replace nulled.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The replace nulled result.</returns>
    public static string ReplaceNulled(string text)
    {
        return text.Replace("(null)", string.Empty).Trim();
    }

    /// <summary>
    /// Shortcut for control.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The shortcut for control result.</returns>
    public static string ShortcutForControl(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in text)
        {
            if (char.IsUpper(item))
            {
                stringBuilder.Append(item.ToString().ToLower());
            }
        }
        return stringBuilder.ToString();
    }

    // Gets default value for type T. Not compatible with default operator - must cast manually
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values
    /// <summary>
    /// Default value for type t t.
    /// </summary>
    /// <typeparam name="T">The type T.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="convertTypeShortcutFullNameToShortcut">The convert type shortcut full name to shortcut.</param>
    /// <returns>The default value for type t t result.</returns>
    public static object DefaultValueForTypeT<T>(T value, Func<string, string> convertTypeShortcutFullNameToShortcut)
    {
        var typeName = value!.GetType().FullName;
        if (typeName!.Contains("."))
        {
            typeName = convertTypeShortcutFullNameToShortcut(typeName);
        }
        switch (typeName)
        {
            case "string":
                return string.Empty;
            case "bool":
                return false;
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return -1;
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return 0;
            case "DateTime":
                return "new(1900, 1, 1)";
            case "byte[]":
                return null!;
            case "Guid":
                return Guid.Empty;
            case "char":
                throw new Exception("Unsupported type");
        }
        throw new Exception("Unsupported type");
    }

    /// <summary>
    /// Default value for type.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <param name="convertTypeShortcutFullNameToShortcut">The convert type shortcut full name to shortcut.</param>
    /// <returns>The default value for type result.</returns>
    public static string DefaultValueForType(string typeName, Func<string, string> convertTypeShortcutFullNameToShortcut)
    {
        if (typeName.Contains("."))
        {
            typeName = convertTypeShortcutFullNameToShortcut(typeName);
        }
        switch (typeName)
        {
            case "string":
                return "\"" + "\"";
            case "bool":
                return "false";
            case "float":
            case "double":
            case "int":
            case "long":
            case "short":
            case "decimal":
            case "sbyte":
                return "-1";
            case "byte":
            case "ushort":
            case "uint":
            case "ulong":
                return "0";
            case "DateTime":
                return "SqlServerHelper.DateTimeMinVal";
            case "byte[]":
                return "null";
            case "Guid":
                return "Guid.Empty";
            case "char":
                throw new Exception("Unsupported type");
            default:
                // For types like Dictionary<int,int>
                return "new " + typeName + "()";
        }
    }

    /// <summary>
    /// Replace readonly to const.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The replace readonly to const result.</returns>
    public static string ReplaceReadonlyToConst(string text)
    {
        text = text.Replace("static readonly", "const");
        text = text.Replace("readonly", "const");
        return text;
    }
}
