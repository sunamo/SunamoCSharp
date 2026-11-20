namespace SunamoCSharp;

public static partial class CSharpHelper
{
    /// <summary>
    /// Get ctor inner.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="values">The values.</param>
    /// <returns>The get ctor inner result.</returns>
    public static string GetCtorInner(int tabCount, IList values)
    {
        const string assignVariable = "this.{0} = {0};";
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in values)
        {
            csg.AppendLine(tabCount, string.Format(assignVariable, item));
        }

        return csg.ToString().Trim();
    }

    /// <summary>
    /// Get array.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="arrayName">The array name.</param>
    /// <returns>The get array result.</returns>
    public static string GetArray(List<string> input, string arrayName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", arrayName, input);
        return generator.ToString();
    }

    /// <summary>
    /// Get list.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="listName">The list name.</param>
    /// <returns>The get list result.</returns>
    public static string GetList(List<string> input, string listName)
    {
        CSharpGenerator generator = new CSharpGenerator();
        generator.List(0, "string", listName, input);
        return generator.ToString();
    }

    /// <summary>
    /// Remove regions.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>The remove regions result.</returns>
    public static List<string> RemoveRegions(List<string> lines)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("#" + "region" + " ") || line.StartsWith("#endregion"))
            {
                lines.RemoveAt(i);
            }
        }

        return lines;
    }

    /// <summary>
    /// Get consts.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="toCamelConvention">The to camel convention.</param>
    /// <returns>The get consts result.</returns>
    public static string GetConsts(List<string> list, bool? toCamelConvention)
    {
        return GetConsts(null!, list, toCamelConvention);
    }

    /// <summary>
    /// Get consts.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <param name="list">The list.</param>
    /// <param name="toCamelConventionFirstCharLower">The to camel convention first char lower.</param>
    /// <returns>The get consts result.</returns>
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower)
    {
        return GetConsts(names, list, toCamelConventionFirstCharLower, Types.StringType);
    }

    // A1 can be null
    // GenerateConstants - const without value
    // GetConsts - static readonly with value
    /// <summary>
    /// Get consts.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <param name="list">The list.</param>
    /// <param name="toCamelConventionFirstCharLower">The to camel convention first char lower.</param>
    /// <param name="type">The type.</param>
    /// <returns>The get consts result.</returns>
    public static string GetConsts(List<string> names, List<string> list, bool? toCamelConventionFirstCharLower, Type type)
    {
        bool addHyphensToValue = true;
        if (type != Types.StringType)
        {
            addHyphensToValue = false;
        }

        if (names != null)
        {
            ThrowEx.DifferentCountInLists("names", names, "list", list);
        }

        CSharpGenerator csg = new CSharpGenerator();
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            string name = item;
            if (names != null)
            {
                name = names[i];
            }

            if (toCamelConventionFirstCharLower.HasValue)
            {
                if (toCamelConventionFirstCharLower.Value)
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
                else
                {
                    name = ConvertPascalConvention.ToConvention(name);
                }
            }

            csg.Field(0, AccessModifiers.Public, true, VariableModifiers.ReadOnly, ConvertTypeShortcutFullName.ToShortcut(type.FullName!, false), name, addHyphensToValue, item);
        }

        var result = csg.ToString();
        return result;
    }

    // GenerateConstants - const without value
    // GetConsts - static readonly with value
    /// <summary>
    /// Generate constants.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="changeInput">The change input.</param>
    /// <param name="input">The input.</param>
    /// <returns>The generate constants result.</returns>
    public static string GenerateConstants(int tabCount, Func<string, string> changeInput, List<string> input)
    {
        CSharpGenerator csg = new CSharpGenerator();
        foreach (var item in input)
        {
            string name = item;
            if (changeInput != null)
            {
                name = changeInput(item);
            }

            csg.Field(tabCount, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", name, true, item);
        }

        return csg.ToString();
    }

    /// <summary>
    /// Trim end.
    /// </summary>
    /// <param name="sf">The sf.</param>
    /// <param name="toTrim">The to trim.</param>
    /// <returns>The trim end result.</returns>
    public static List<string> TrimEnd(List<string> sf, params char[] toTrim)
    {
        for (int i = 0; i < sf.Count; i++)
        {
            sf[i] = sf[i].TrimEnd(toTrim);
        }

        return sf;
    }

    // value - data type
    // key - name
    /// <summary>
    /// Parse fields.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>The parse fields result.</returns>
    public static Dictionary<string, string> ParseFields(List<string> lines)
    {
        CA.RemoveStringsEmpty2(lines);
        CAChangeContent.ChangeContent0(null!, lines, e => SHParts.RemoveAfterFirst(e, '='));
        CA.TrimEnd(lines, ';');
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (var item in lines)
        {
            var parameter = SHSplit.SplitByWhiteSpaces(item);
            var count = parameter.Count;
            StringBuilder sb2 = new StringBuilder(parameter[count - 1]);
            sb2[0] = char.ToLower(sb2[0]);
            result.Add(sb2.ToString(), parameter[count - 2]);
        }

        return result;
    }

    // 0 - item.Value
    // 1 - CSharpHelperSunamo.DefaultValueForType(item.Value)
    // 2 - item.Key
    // 3 - SH.FirstCharUpper(item.Key)
    const string tProperty = @"{0} {2} = {1};
    public {0} {3} { get { return {2}; } set { {2} = value; OnPropertyChanged(" + "\"{3}\"); } }" + @"
";
    /// <summary>
    /// Generate properties.
    /// </summary>
    /// <param name="argument">The argument.</param>
    /// <returns>The generate properties result.</returns>
    public static string GenerateProperties(GeneratePropertiesArgs argument)
    {
        var lines = argument.Input;
        Dictionary<string, string>? data = null;
        if (argument.AllStrings)
        {
            data = new Dictionary<string, string>(lines.Count);
            foreach (var item in lines)
            {
                var parameter = ConvertPascalConvention.ToConvention(item);
                StringBuilder sb2 = new StringBuilder(parameter);
                sb2[0] = char.ToUpper(sb2[0]);
                data.Add(sb2.ToString(), "string");
            }
        }
        else
        {
            data = ParseFields(lines);
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in data)
        {
            StringBuilder sb2 = new StringBuilder(item.Key);
            sb2[0] = char.ToUpper(sb2[0]);
            stringBuilder.AppendLine( /*SHFormat.Format3*/string.Format((string)tProperty, item.Value, CSharpHelperSunamo.DefaultValueForType(item.Value, ConvertTypeShortcutFullName.ToShortcut), item.Key, sb2.ToString()));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Line with class.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="mustDerive">The must derive.</param>
    /// <returns>The line with class result.</returns>
    public static string? LineWithClass(List<string> lines, bool mustDerive)
    {
        foreach (var item in lines)
        {
            if (item.Contains("class "))
            {
                if (mustDerive)
                {
                    if (item.Contains(":"))
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    static Type type = typeof(CSharpHelper);
}
