namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

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
        // The path would be useful here because of FS.CiStorageFile
        // however I do not know whether StorageFolder is used, so it will work even without it
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