namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

/// <summary>
/// Base class of code generators that provides indentation and braces.
/// </summary>
public abstract class GeneratorCodeAbstract
{
    // Use ToString() instead of public access
    protected string Final = "";

    protected InstantSB sb = new(" ");
    public XmlDoc xmlDoc;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public GeneratorCodeAbstract()
    {
        xmlDoc = new XmlDoc(sb);
    }

    /// <summary>
    /// Adds the indentation and the text.
    /// </summary>
    public void AddTab2(int tabCount, string text)
    {
        sb.AddItem(AddTab(tabCount, text));
    }

    // Ends a brace and adds a new line. After calling this method for elements like methods, properties, or constructors, it's recommended to also call sb.AppendLine().
    /// <summary>
    /// Appends the closing brace.
    /// </summary>
    public void EndBrace(int tabCount)
    {
        AddTab(tabCount);
        sb.AppendLine("}");
    }

    // Starts a brace. This is the only method here that adds a new line at the beginning.
    /// <summary>
    /// Appends the opening brace.
    /// </summary>
    public void StartBrace(int tabCount)
    {
        // Line always ending previous command
        AddTab(tabCount);
        sb.AppendLine("{");
    }

    /// <summary>
    /// Appends the opening parenthesis.
    /// </summary>
    public void StartParenthesis()
    {
        sb.AddItem("(");
    }

    /// <summary>
    /// Appends the closing parenthesis.
    /// </summary>
    public void EndParenthesis()
    {
        sb.AddItem(")");
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine()
    {
        sb.AppendLine();
    }

    // Appends a formatted line with tabs
    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AppendLine(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AppendLine(AddTab(tabCount, format));
    }

    // Appends formatted text with tabs (without newline)
    /// <summary>
    /// Appends the text.
    /// </summary>
    public void Append(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AddItem(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AddItem(AddTab(tabCount, format));
    }

    // Returns the generated code and resets the string builder
    /// <summary>
    /// Returns the generated text.
    /// </summary>
    public override string ToString()
    {
        var result = sb.ToString();
        sb = new InstantSB(" ");
        return result;
    }

    /// <summary>
    /// Adds the indentation.
    /// </summary>
    public void AddTab(int tabCount)
    {
        for (var i = 0; i < tabCount; i++) sb.AddRaw("\t");
    }

    // Adds tabs to each line of the text
    /// <summary>
    /// Adds the indentation.
    /// </summary>
    public static string AddTab(int tabCount, string text)
    {
        var lines = SHGetLines.GetLines(text);
        for (var i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Trim();
            for (var tabIndex = 0; tabIndex < tabCount; tabIndex++) lines[i] = "\t" + lines[i];
        }

        var result = string.Join(Environment.NewLine, lines);
        return result;
    }
}