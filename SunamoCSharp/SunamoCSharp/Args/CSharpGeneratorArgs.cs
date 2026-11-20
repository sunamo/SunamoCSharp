namespace SunamoCSharp.Args;

/// <summary>
/// Represents the c sharp generator args type.
/// </summary>
public class CSharpGeneratorArgs
{
    /// <summary>
    /// Gets or sets the add hyphens.
    /// </summary>
    public bool AddHyphens { get; set; } = false;

    /// <summary>
    /// Gets or sets the create instance.
    /// </summary>
    public bool CreateInstance { get; set; } = false;

    /// <summary>
    /// Gets or sets the also field.
    /// </summary>
    public bool AlsoField { get; set; } = false;

    /// <summary>
    /// Gets or sets the adding value.
    /// </summary>
    public bool AddingValue { get; set; } = false;

    /// <summary>
    /// Gets or sets the use CA.
    /// </summary>
    public bool UseCA { get; set; } = true;

    /// <summary>
    /// Gets or sets the split key with.
    /// </summary>
    public string? SplitKeyWith { get; set; } = null;

    /// <summary>
    /// Gets or sets the check for null.
    /// </summary>
    public bool CheckForNull { get; set; } = false;
}
