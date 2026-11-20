namespace SunamoCSharp;

public static partial class CSharpHelper
{
    /// <summary>
    /// Get inner content of code element class.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>The get inner content of code element class result.</returns>
    public static string GetInnerContentOfCodeElementClass(List<string> lines)
    {
        if (IsEmptyOrCommented(lines))
        {
            return string.Empty;
        }

        var dxF = lines.IndexOf("{");
        ;
        var dxL = lines.LastIndexOf("}");
        return SHJoin.JoinNL(lines.Skip(dxF + 1).Take(dxL - dxF).ToList());
    }

    // Returns true even if it contains code without a semicolon (e.g. an empty class)
    /// <summary>
    /// Is empty commented or only with namespace.
    /// </summary>
    /// <param name="fnwoe">The fnwoe.</param>
    /// <param name="linesOriginal">The lines original.</param>
    /// <param name="RemoveBetweenIfAndEndif">The remove between if and endif.</param>
    /// <param name="csWithSharpIf">The cs with sharp if.</param>
    /// <returns>The is empty commented or only with namespace result.</returns>
    public static bool IsEmptyCommentedOrOnlyWithNamespace(string fnwoe, List<string> linesOriginal, Action<List<string>> RemoveBetweenIfAndEndif, List<string> csWithSharpIf)
    {
        var lines = linesOriginal.ToList();
        if (csWithSharpIf.Contains(fnwoe))
        {
            return false;
        }

        if (RemoveBetweenIfAndEndif != null)
        {
            RemoveBetweenIfAndEndif(lines);
        }

        CA.Trim(lines);
        CA.RemoveNullEmptyWs(lines);
        lines = RemoveComments(lines);
        lines = lines.Where(data => !data.StartsWith("namespace")).ToList();
        lines = lines.Where(data => !data.StartsWith("using")).ToList();
        lines = lines.Where(data => data.EndsWith(";") || data.EndsWith(",")).ToList();
        if (!lines.Any())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Is empty or commented.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>The is empty or commented result.</returns>
    public static bool IsEmptyOrCommented(List<string> lines)
    {
        foreach (var item in lines)
        {
            var data = item.Trim();
            if (!data.StartsWith("//") && data != string.Empty)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Remove imports usings.
    /// </summary>
    /// <param name="l2">The l 2.</param>
    /// <param name="imports">The imports.</param>
    public static void RemoveImportsUsings(List<string> l2, bool imports)
    {
        string k = Using;
        if (imports)
        {
            k = Import;
        }

        for (int i = 0; i < l2.Count; i++)
        {
            var line = l2[i].Trim();
            if (line.StartsWith(k) || line == string.Empty)
            {
                l2[i] = string.Empty;
            }
            else
            {
                break;
            }
        }

        CA.RemoveEmptyLinesToFirstNonEmpty(l2);
    }

    /// <summary>
    /// Usings.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="remove">The remove.</param>
    /// <returns>The usings result.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, bool remove = false)
    {
        return Usings(lines, Using, remove);
    }

    /// <summary>
    /// Imports.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="remove">The remove.</param>
    /// <returns>The imports result.</returns>
    public static CollectionWithoutDuplicatesDC<string> Imports(List<string> lines, bool remove = false)
    {
        return Usings(lines, Import, remove);
    }

    /// <summary>
    /// Usings.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="keyword">The keyword.</param>
    /// <param name="remove">The remove.</param>
    /// <returns>The usings result.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, string keyword, bool remove = false)
    {
        List<int> removeLines = null!;
        return Usings(lines, keyword, out removeLines, remove);
    }

    /// <summary>
    /// Usings.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="keyword">The keyword.</param>
    /// <param name="removeLines">The remove lines.</param>
    /// <param name="remove">The remove.</param>
    /// <returns>The usings result.</returns>
    public static CollectionWithoutDuplicatesDC<string> Usings(List<string> lines, string keyword, out List<int> removeLines, bool remove = false)
    {
        CollectionWithoutDuplicatesDC<string> usings = new CollectionWithoutDuplicatesDC<string>();
        removeLines = new List<int>();
        int i = -1;
        foreach (var item in lines)
        {
            i++;
            var line = item.Trim();
            if (line != string.Empty)
            {
                if (line.StartsWith(keyword))
                {
                    removeLines.Add(i);
                    usings.Add(line);
                }
                else //if (line.Contains("{"))
                {
                    break;
                }
            }
        }

        if (remove)
        {
            CA.RemoveLines(lines, removeLines);
        }

        return usings;
    }

    /// <summary>
    /// Get dictionary values from two list.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <param name="chars">The chars.</param>
    /// <returns>The get dictionary values from two list result.</returns>
    public static string GetDictionaryValuesFromTwoList(List<string> names, List<string> chars)
    {
        return CSharpHelper.GetDictionaryValuesFromTwoList<string, string>(2, "a", names, chars, new CSharpGeneratorArgs { SplitKeyWith = "," });
    }

    /// <summary>
    /// Get dictionary values from dictionary.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>The get dictionary values from dictionary result.</returns>
    public static string GetDictionaryValuesFromDictionary(Dictionary<string, string> data)
    {
        return CSharpHelper.GetDictionaryValuesFromDictionary<string, string>(0, "name", data);
    }

    /// <summary>
    /// Get summary xml documentation.
    /// </summary>
    /// <param name="cs">The cs.</param>
    /// <returns>The get summary xml documentation result.</returns>
    public static string GetSummaryXmlDocumentation(List<string> cs)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < cs.Count; i++)
        {
            var line = cs[i];
            if (line.StartsWith(CodeElementsConstants.XmlDocumentationCsharp))
            {
                line = line.TrimStart('/'); //SHTrim.TrimStart(line, CodeElementsConstants.XmlDocumentationCsharp).Trim();
                stringBuilder.AppendLine(line);
                if (line.Contains("</summary>"))
                {
                    break;
                }
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Create consts for search uris.
    /// </summary>
    /// <param name="uris">The uris.</param>
    /// <returns>The create consts for search uris result.</returns>
    public static string CreateConstsForSearchUris(List<string> uris)
    {
        CSharpGenerator csg = new CSharpGenerator();
        // In key name of const, in value value
        Dictionary<string, string> dict = new Dictionary<string, string>();
        List<string> all = new List<string>();
        foreach (var item in uris)
        {
            Uri u = new Uri(item);
            string name = u.Host.ToConstantCase().ToPascalCase(); //CaseDotNet.CaseConverter.PascalCase.ConvertCase(u.Host); //ConvertPascalConvention.ToConvention(u.Host);
            dict.Add(name, item);
            all.Add(name);
        }

        CreateConsts(csg, dict);
        csg.List(2, "string", "All", all, new CSharpGeneratorArgs { AddHyphens = false });
        return csg.ToString();
    }

    /// <summary>
    /// Create consts.
    /// </summary>
    /// <param name="dict">The dict.</param>
    /// <returns>The create consts result.</returns>
    public static string CreateConsts(Dictionary<string, string> dict)
    {
        CSharpGenerator csg = new CSharpGenerator();
        return CreateConsts(csg, dict);
    }

    private static string CreateConsts(CSharpGenerator csg, Dictionary<string, string> dict)
    {
        foreach (var item in dict)
        {
            csg.Field(2, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", item.Key, true, item.Value);
        }

        return csg.ToString();
    }

    /// <summary>
    /// Dictionary with class key value.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="randomValue">The random value.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>The dictionary with class key value result.</returns>
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue, CSharpGeneratorArgs? argument = null) where Key : notnull
    {
        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromRandomValue<Key, Value>(0, nameDictionary, keys, randomValue, argument);
        var inner = GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        genCS.Ctor(1, ModifiersConstructor.Private, nameDictionary, inner);
        genCS.EndBrace(0);
        return genCS.ToString();
    }

    /// <summary>
    /// Dictionary with class key value.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="data">The data.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>The dictionary with class key value result.</returns>
    public static string DictionaryWithClass<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> data, CSharpGeneratorArgs? argument = null) where Key : notnull
    {
        if (argument == null)
        {
            argument = new CSharpGeneratorArgs { AddingValue = false };
        }

        CSharpGenerator genCS = new CSharpGenerator();
        genCS.StartClass(0, AccessModifiers.Private, false, nameDictionary);
        genCS.DictionaryFromDictionary<Key, Value>(0, nameDictionary, data, argument);
        var inner = GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, data);
        genCS.Ctor(1, ModifiersConstructor.Private, nameDictionary, inner);
        genCS.EndBrace(0);
        return genCS.ToString();
    }

    /// <summary>
    /// Get dictionary values from dictionary key value.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="dict">The dict.</param>
    /// <returns>The get dictionary values from dictionary key value result.</returns>
    public static string GetDictionaryValuesFromDictionary<Key, Value>(int tabCount, string nameDictionary, Dictionary<Key, Value> dict) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromDictionary<Key, Value>(tabCount, nameDictionary, dict);
        return csg.ToString();
    }
}
