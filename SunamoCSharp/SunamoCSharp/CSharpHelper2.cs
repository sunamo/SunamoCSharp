namespace SunamoCSharp;

public static partial class CSharpHelper
{
    /// <summary>
    /// Get dictionary values from two list key value.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="values">The values.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>The get dictionary values from two list key value result.</returns>
    public static string GetDictionaryValuesFromTwoList<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, List<Value> values, CSharpGeneratorArgs argument) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromTwoList<Key, Value>(tabCount, nameDictionary, keys, values, argument);
        return csg.ToString();
    }

    /// <summary>
    /// Get dictionary values from random value key value.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="randomValue">The random value.</param>
    /// <returns>The get dictionary values from random value key value result.</returns>
    public static string GetDictionaryValuesFromRandomValue<Key, Value>(int tabCount, string nameDictionary, List<Key> keys, Func<Value> randomValue) where Key : notnull
    {
        CSharpGenerator csg = new CSharpGenerator();
        csg.GetDictionaryValuesFromRandomValue<Key, Value>(tabCount, nameDictionary, keys, randomValue);
        return csg.ToString();
    }

    /// <summary>
    /// Remove xml doc comments except summary.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="removedAnything">The removed anything.</param>
    /// <returns>The remove xml doc comments except summary result.</returns>
    public static string RemoveXmlDocCommentsExceptSummary(List<string> list, ref bool removedAnything)
    {
        removedAnything = false;
        const string argument = @"/// <returns></returns>";
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == argument)
            {
                removedAnything = true;
                list.RemoveAt(i);
            }
        }

        return string.Join(Environment.NewLine, list); //string.Join(Environment.NewLine, list);
    }

    /// <summary>
    /// Remove xml doc comments.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The remove xml doc comments result.</returns>
    public static string RemoveXmlDocComments(List<string> list)
    {
        ThrowEx.NotImplementedMethod();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "///");
        }

        CA.TrimWhereIsOnlyWhitespace(list);
        var text = string.Join(Environment.NewLine, list);
        CA.DoubleOrMoreMultiLinesToSingle(ref text);
        return text;
    }

    /// <summary>
    /// Remove comments.
    /// </summary>
    /// <param name="listOrString">The list or string.</param>
    /// <param name="line">The line.</param>
    /// <param name="block">The block.</param>
    /// <param name="keepLinesNumbers">The keep lines numbers.</param>
    /// <returns>The remove comments result.</returns>
    public static string RemoveComments(string listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return RemoveCommentsKeepLines(listOrString);
        }

        return SHJoin.JoinNL(RemoveComments(SHGetLines.GetLines(listOrString), line, block));
    }

    private static string RemoveCommentsKeepLines(string input)
    {
        string output = "";
        using (StringReader reader = new(input))
        {
            bool inSingleLineComment = false;
            bool inMultiLineComment = false;
            int currentChar;
            while ((currentChar = reader.Read()) != -1)
            {
                char current = (char)currentChar;
                char next = (char)reader.Peek();
                if (inSingleLineComment)
                {
                    if (current == '\n')
                    {
                        inSingleLineComment = false;
                        output += current;
                    }
                }
                else if (inMultiLineComment)
                {
                    if (current == '*' && next == '/')
                    {
                        inMultiLineComment = false;
                        reader.Read(); // We read '/'
                    }
                    else if (current == '\n')
                    {
                        output += current; // We add a new line to the output
                    }
                }
                else
                {
                    if (current == '/' && next == '/')
                    {
                        inSingleLineComment = true;
                        reader.Read(); // We read '/'
                    }
                    else if (current == '/' && next == '*')
                    {
                        inMultiLineComment = true;
                        reader.Read(); // We read '*'
                    }
                    else
                    {
                        output += current;
                    }
                }
            }
        }

        return output;
    }

    /// <summary>
    /// Remove comments.
    /// </summary>
    /// <param name="listOrString">The list or string.</param>
    /// <param name="line">The line.</param>
    /// <param name="block">The block.</param>
    /// <param name="keepLinesNumbers">The keep lines numbers.</param>
    /// <returns>The remove comments result.</returns>
    public static List<string> RemoveComments(List<string> listOrString, bool line = true, bool block = true, bool keepLinesNumbers = false)
    {
        if (keepLinesNumbers)
        {
            return SHGetLines.GetLines(RemoveCommentsKeepLines(SHJoin.JoinNL(listOrString)));
        }

        if (line)
        {
            listOrString = RemoveLineComments(listOrString);
        }

        if (block)
        {
            listOrString = SHGetLines.GetLines(RemoveBlockComments(string.Join(Environment.NewLine, listOrString)));
        }

        // how could I be such an idiot to put the argument here - what is there to trim here?
        return listOrString;
    }

    // Direct edit
    /// <summary>
    /// Remove line comments.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The remove line comments result.</returns>
    public static List<string> RemoveLineComments(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list[i] = SHParts.RemoveAfterFirst(list[i], "//");
        }

        CA.RemoveStringsEmpty2(list);
        return list;
    }

    const string blockComments = @"/\*(.*?)\*/";
    const string lineComments = @"//(.*?)\r?\n";
    const string strings = @"""((\\[^\n]|[^""\n])*)""";
    const string verbatimStrings = @"@(""[^""]*"")+";

    /// <summary>
    /// Remove block comments.
    /// </summary>
    /// <param name="str">The str.</param>
    /// <returns>The remove block comments result.</returns>
    public static string RemoveBlockComments(string str)
    {
        str = Regex.Replace(str, blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings, me =>
        {
            if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                return me.Value.StartsWith("//") ? Environment.NewLine : "";
            // Keep the literal strings
            return me.Value;
        }, RegexOptions.Singleline);
        return str;
    }

    /// <summary>
    /// Get dictionary string object value.
    /// </summary>
    /// <typeparam name="Value">The type Value.</typeparam>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="values">The values.</param>
    /// <param name="nameDictionary">The name dictionary.</param>
    /// <param name="argument">The argument.</param>
    /// <returns>The get dictionary string object value result.</returns>
    public static string GetDictionaryStringObject<Value>(int tabCount, List<string> keys, List<Value> values, string nameDictionary, CSharpGeneratorArgs argument)
    {
        int localTabCount = 0;
        CSharpGenerator gen = new CSharpGenerator();
        gen.DictionaryFromTwoList<string, Value>(tabCount, nameDictionary, keys, values, argument);
        if (argument.CheckForNull)
        {
            gen.If(localTabCount, nameDictionary + " " + "== null");
        }

        gen.GetDictionaryValuesFromDictionary<string, Value>(localTabCount, nameDictionary, DictionaryHelper.GetDictionary<string, Value>(keys, values));
        if (argument.CheckForNull)
        {
            gen.EndBrace(localTabCount);
        }

        string result = gen.ToString();
        return result;
    }

    /// <summary>
    /// Default value for type sq lite.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The default value for type sq lite result.</returns>
    public static string DefaultValueForTypeSqLite(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "TEXT":
                return "\"";
            case "INTEGER":
                return int.MaxValue.ToString();
            case "REAL":
                return "0.0";
            case "DATETIME":
                // MinValue was originally here because of SQLite but I agreed with myself that I will no longer use SQLite, so with this argument I save value code work text MSSQL
                return "DateTime.MinValue";
            case "BLOB":
                // Supported type only in desktop applications where there is no sbf folder
                return "null";
            default:
                ThrowEx.NotImplementedCase(type);
                break;
        }

        throw new Exception("Nepodporovaný typ");
    }
}
