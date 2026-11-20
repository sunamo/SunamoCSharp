namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

/// <summary>
/// Base class of a list that ignores duplicated values.
/// </summary>
public abstract class CollectionWithoutDuplicatesBaseDC<T>
{
    /// <summary>
    /// Gets or sets the collection.
    /// </summary>
    public List<T> Collection { get; set; } = null!;

    /// <summary>
    /// Gets or sets the string representations.
    /// </summary>
    public List<string> StringRepresentations { get; set; } = null!;

    private bool? _allowNull = false;

    /// <summary>
    /// The allow null.
    /// </summary>
    public bool? AllowNull
    {
        get => _allowNull;
        set
        {
            _allowNull = value;
            if (value.HasValue && value.Value)
            {
                StringRepresentations = new List<string>(InitialCapacity);
            }
        }
    }

    /// <summary>
    /// Gets or sets the break on construction.
    /// </summary>
    public static bool BreakOnConstruction { get; set; } = false;

    private int InitialCapacity { get; set; } = 10000;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesBaseDC()
    {
        if (BreakOnConstruction)
        {
            System.Diagnostics.Debugger.Break();
        }
        Collection = new List<T>();
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesBaseDC(int capacity)
    {
        this.InitialCapacity = capacity;
        Collection = new List<T>(capacity);
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesBaseDC(IList<T> initialList)
    {
        Collection = new List<T>(initialList.ToList());
    }

    /// <summary>
    /// Adds the value.
    /// </summary>
    public bool Add(T value)
    {
        bool result = false;
        var containsResult = Contains(value);
        if (containsResult.HasValue)
        {
            if (!containsResult.Value)
            {
                Collection.Add(value);
                result = true;
            }
        }
        else
        {
            if (!AllowNull.HasValue)
            {
                Collection.Add(value);
                result = true;
            }
        }
        if (result)
        {
            if (IsComparingByString())
            {
                StringRepresentations.Add(ItemString);
            }
        }
        return result;
    }

    /// <summary>
    /// Determines whether the values are compared by their string representation.
    /// </summary>
    protected abstract bool IsComparingByString();

    /// <summary>
    /// Gets or sets the item string.
    /// </summary>
    protected string ItemString { get; set; } = null!;

    /// <summary>
    /// Checks whether the value is already present.
    /// </summary>
    public abstract bool? Contains(T value);

    /// <summary>
    /// Adds the value and returns its index.
    /// </summary>
    public abstract int AddWithIndex(T value);

    /// <summary>
    /// Returns index of the value or -1.
    /// </summary>
    public abstract int IndexOf(T value);

    private List<T> WasNotAdded { get; set; } = new List<T>();
}