namespace SunamoCSharp._sunamo;

/// <summary>
/// Arguments for generating properties.
/// </summary>
public class GeneratePropertiesArgs
{
    public List<string> Input { get; set; } = null!;

    public bool AllStrings { get; set; } = false;
}
