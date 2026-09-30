namespace SunamoCSharp._sunamo.SunamoDevCodeBase;

// Must have always entered both from and to
// None of event could have unlimited time!
/// <summary>
/// Range of long values.
/// </summary>
public class FromToDC : FromToTSHDC<long>
{

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public FromToDC()
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    private FromToDC(bool empty)
    {
        this.empty = empty;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public FromToDC(long from, long to, FromToUseDC ftUse = FromToUseDC.DateTime)
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
}