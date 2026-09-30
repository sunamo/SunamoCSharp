namespace SunamoCSharp._sunamo;

/// <summary>
/// Helpers for working with dictionaries.
/// </summary>
internal class DictionaryHelper
{

    /// <summary>
    /// Creates a dictionary from lists of keys and values.
    /// </summary>
    internal static Dictionary<Key, Value> GetDictionary<Key, Value>(List<Key> keys, List<Value> values) where Key : notnull
    {
        ThrowEx.DifferentCountInLists("keys", keys.Count, "values", values.Count);
        Dictionary<Key, Value> result = new Dictionary<Key, Value>();
        for (int i = 0; i < keys.Count; i++)
        {
            result.Add(keys[i], values[i]);
        }
        return result;
    }

    /// <summary>
    /// Returns value of the first item of the dictionary.
    /// </summary>
    internal static Value? GetFirstItemValue<Key, Value>(Dictionary<Key, Value> dict) where Key : notnull
    {
        foreach (var item in dict)
        {
            return item.Value;
        }

        return default(Value);
    }
}
