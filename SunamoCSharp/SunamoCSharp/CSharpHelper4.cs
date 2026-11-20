namespace SunamoCSharp;

public static partial class CSharpHelper
{
    // CsKeywordsList is internal - public entry point for package consumers
    /// <summary>
    /// Initializes the C# keyword lists (public entry point, because CsKeywordsList is internal).
    /// </summary>
    public static void InitKeywords() => CsKeywordsList.Init();

    /// <summary>
    /// Default value for type object.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The default value for type object result.</returns>
    public static object DefaultValueForTypeObject(string type)
    {
        if (type.Contains("."))
        {
            type = ConvertTypeShortcutFullName.ToShortcut(type);
        }

        switch (type)
        {
            case "string":
                return "\"" + "\"";
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
                // MinValue was originally here because of SQLite but I agreed with myself that I will no longer use SQLite, so with this argument I save value code work text MSSQL
                return "new(1900, 1, 1)";
            case "char":
                throw new Exception(type);
            case "byte" + "[]":
                // Supported type only in desktop applications where there is no sbf folder
                return null!;
        }

        throw new Exception("Nepodporovaný typ");
    }

    /// <summary>
    /// Wrap with region.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="regionName">The region name.</param>
    /// <returns>The wrap with region result.</returns>
    public static string WrapWithRegion(string text, string regionName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#region ");
        stringBuilder.AppendLine(regionName);
        stringBuilder.AppendLine(text);
        stringBuilder.AppendLine("#endregion");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Is keyword.
    /// </summary>
    /// <param name="con">The con.</param>
    /// <returns>The is keyword result.</returns>
    public static bool IsKeyword(string con)
    {
        if (CsKeywordsList.Modifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.AccessModifier.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Statement.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.MethodParameter.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Namespace.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Operator.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Access.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Literal.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Type.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Contextual.Contains(con))
        {
            return true;
        }

        if (CsKeywordsList.Query.Contains(con))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Wrap with quote list.
    /// </summary>
    /// <param name="tValue">The t value.</param>
    /// <param name="valueS">The value s.</param>
    /// <returns>The wrap with quote list result.</returns>
    public static string WrapWithQuoteList(Type tValue, IList valueS)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in valueS)
        {
            string value = item!.ToString()!;
            WrapWithQuote(tValue, ref value);
            stringBuilder.Append(value + ",");
        }

        return stringBuilder.ToString().TrimEnd(',');
    }

    /// <summary>
    /// Wrap with quote.
    /// </summary>
    /// <param name="tKey">The t key.</param>
    /// <param name="keyS">The key s.</param>
    public static void WrapWithQuote(Type tKey, ref string keyS)
    {
        if (tKey == Types.StringType)
        {
            keyS = SH.WrapWithQm(keyS);
        }
        else if (tKey == Types.CharType)
        {
            keyS = SH.WrapWith(keyS, "\'");
        }
        else
        {
        }
    }
}
