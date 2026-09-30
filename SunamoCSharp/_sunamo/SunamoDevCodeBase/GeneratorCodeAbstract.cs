namespace SunamoCSharp._sunamo;

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

    // EN: Ends a brace and adds a new line. After calling this method for elements like methods, properties, or constructors, it's recommended to also call sb.AppendLine().
    // CZ: Ukončí složenou závorku a přidá nový řádek. Za voláním této metody pokud ukončuje nějaký celek jako jsou metody, vlastnosti nebo konstruktor je vhodné volat ještě sb.AppendLine().
    /// <summary>
    /// Appends the closing brace.
    /// </summary>
    public void EndBrace(int tabCount)
    {
        //sb.AppendLine();
        AddTab(tabCount);
        //sb.AppendLine();
        sb.AppendLine("}");
    }

    // EN: Starts a brace. This is the only method here that adds a new line at the beginning.
    // CZ: Přidá nový řádek, složenou závorku. Je to jediná zdejší metoda která na začátku přidává nový řádek.
    /// <summary>
    /// Appends the opening brace.
    /// </summary>
    public void StartBrace(int tabCount)
    {
        // Line always ending previous command
        //sb.AppendLine();
        AddTab(tabCount);
        sb.AppendLine("{");
        //sb.AppendLine();
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

    // EN: Appends a formatted line with tabs
    // CZ: Přidá formátovaný řádek s tabulátory
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

    // EN: Appends formatted text with tabs (without newline)
    // CZ: Přidá formátovaný text s tabulátory (bez nového řádku)
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

    // EN: Returns the generated code and resets the string builder
    // CZ: Vrátí vygenerovaný kód a resetuje string builder
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
        //tabCount += 1;
        for (var i = 0; i < tabCount; i++) sb.AddRaw("\t");
    }

    // EN: Adds tabs to each line of the text
    // CZ: Přidá tabulátory na začátek každého řádku textu
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
