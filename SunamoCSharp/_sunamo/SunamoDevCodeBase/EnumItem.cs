namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

/// <summary>
/// Item of a generated enum.
/// </summary>
public class EnumItem
{
    /// <summary>
    /// Gets or sets the hex.
    /// </summary>
    public string Hex { get; set; } = "";

    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    public Dictionary<string, string>? Attributes { get; set; } = null;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}