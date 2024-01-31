namespace SunamoCSharp.Helpers;

public partial class CSharpHelperSunamo
{
    public static string IsAllCsprojAndSlnRightInHiearchy(string path)
    {
        var csproj = FS.GetFiles(path, "*.csproj", true);
        var sln = FS.GetFiles(path, "*.sln", true);

        //List<string> foldersWithSlnOk = new List<string>();
        List<string> foldersWithSlnKo = new List<string>();

        for (int i = sln.Count - 1; i >= 0; i--)
        {
            var item = sln[i];

            if (item == @"E:\vs\Projects\_ut2\Wpf.Tests\Wpf.Tests.sln")
            {

            }

            if (!IsOnlyInSpecialOrProjectFolders(sln[i]))
            {
                foldersWithSlnKo.Add(sln[i]);
                sln.RemoveAt(i);
            }
        }

        sln = CAChangeContent.ChangeContent0(null, sln, FS.GetDirectoryName);

        for (int i = csproj.Count - 1; i >= 0; i--)
        {
            var csprojFolder = FS.GetDirectoryName(csproj[i]);
            var slnFolder = FS.GetDirectoryName(csprojFolder);

            if (sln.Contains(slnFolder))
            {
                csproj.RemoveAt(i);
            }
        }

        TextOutputGenerator tog = new TextOutputGenerator();

        tog.List(foldersWithSlnKo, "Sln in wrong folder:");
        tog.List(csproj, "Csproj in wrong folder:");

        return tog.ToString();
    }

    private static bool IsOnlyInSpecialOrProjectFolders(string filePath)
    {
        filePath = Path.GetDirectoryName(filePath);

        while (true)
        {
            filePath = Path.GetDirectoryName(filePath);

            if (filePath.Length < 4)
            {
                return false;
            }

            var fn = Path.GetFileName(filePath);

            if (fn.EndsWith("Projects"))
            {
                return true;
            }

            if (!fn.StartsWith("_"))
            {
                return false;
            }
        }

        return false;
    }

    public static FromToList DetectFromToString(string s)
    {
        var oc = SH.ReturnOccurencesOfString(s, AllStrings.qm);
        for (int i = oc.Count - 1; i >= 0; i--)
        {
            if (s[oc[i] - 1] == AllChars.bs)
            {
                oc.RemoveAt(i);
            }
        }

        ThrowEx.IsOdd("oc", oc);

        var ft = new FromToList();
        for (int i = 0; i < oc.Count; i++)
        {
            ft.c.Add(new FromTo(oc[i], oc[++i]));
        }
        return ft;
    }

    public static void IndentAsPreviousLine(List<string> lines)
    {
        string indentPrevious = string.Empty;
        string line = null;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lines.Count; i++)
        {
            line = lines[i];
            if (line.Length > 0)
            {
                if (!char.IsWhiteSpace(line[0]))
                {
                    lines[i] = indentPrevious + lines[i];
                }
                else
                {
                    indentPrevious = SH.GetWhitespaceFromBeginning(sb, line);
                }
            }
        }
    }
    public static bool IsInterface(string item)
    {
        if (item[0] == 'I')
        {
            if (char.IsUpper(item[1]))
            {
                return true;
            }
        }
        return false;
    }

    public static string ReplaceNulled(string s)
    {
        return s.Replace(Consts.nulled, string.Empty).Trim();
    }

    public static string ShortcutForControl(string name)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in name)
        {
            if (char.IsUpper(item))
            {
                sb.Append(item.ToString().ToLower());
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// Its not compatible with default operator
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values
    /// Nonsense, cant type too many different output types to T.
    /// Must cast manually
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "t"></param>
    public static object DefaultValueForTypeT<T>(T t)
    {
        var type = t.GetType().FullName;
        if (type.Contains(AllStrings.dot))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }
        #region Same seria as in Types
        switch (type)
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
                // Původně tu bylo MinValue kvůli SQLite ale dohodl jsem se že SQLite už nebudu používat a proto si ušetřím v kódu práci s MSSQL
                return Consts.DateTimeMinVal;
            case "byte[]":
                // Podporovaný typ pouze v desktopových aplikacích, kde není lsožka sbf
                return null;
            case "Guid":
                return Guid.Empty;
            case "char":
                ThrowEx.Custom("Nepodporovan\u00FD typ");
                break;
        }
        #endregion
        ThrowEx.Custom("Nepodporovan\u00FD typ");
        return null;
    }
}
