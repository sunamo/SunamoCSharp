namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
/// <summary>
/// Helpers for working with file system paths.
/// </summary>
internal partial class FS
{

    /// <summary>
    /// Inserts the text between file name and extension.
    /// </summary>
    internal static string InsertBetweenFileNameAndExtension(string originalPath, string textToInsert)
    {
        //return InsertBetweenFileNameAndExtension<string, string>(originalPath, textToInsert, null);
        // Cesta by se zde hodila kvůli FS.CiStorageFile
        // nicméně StorageFolder nevím zda se používá, takže to bude umět i bez toho
        var originalPathString = originalPath.ToString();
        string fileName = Path.GetFileNameWithoutExtension(originalPathString);
        string extension = Path.GetExtension(originalPathString);
        if (originalPathString.Contains('/') || originalPathString.Contains('\\'))
        {
            string directory = Path.GetDirectoryName(originalPathString)!;
            return Path.Combine(directory, fileName + textToInsert + extension);
        }

        return fileName + textToInsert + extension;
    }
}