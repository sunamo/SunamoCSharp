namespace SunamoCSharp._sunamo;

/// <summary>
/// Throws exceptions built by Exceptions.
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Returns message about collection with odd number of elements.
    /// </summary>
    internal static bool HasOddNumberOfElements(string listName, ICollection list)
    {
        var exceptionFunction = Exceptions.HasOddNumberOfElements;
        return ThrowIsNotNull(exceptionFunction, listName, list);
    }

    /// <summary>
    /// Returns custom exception message.
    /// </summary>
    internal static bool Custom(string message, bool reallyThrow = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? str = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(str, reallyThrow);
    }
    /// <summary>
    /// Returns message about lists with different element counts.
    /// </summary>
    internal static bool DifferentCountInLists<T>(string firstListName, IList<T> firstList, string secondListName, IList<T> secondList)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), firstListName, firstList.Count, secondListName, secondList.Count));
    }

    /// <summary>
    /// Returns message about lists with different element counts.
    /// </summary>
    internal static bool DifferentCountInLists(string firstListName, int firstCount, string secondListName, int secondCount)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), firstListName, firstCount, secondListName, secondCount));
    }

    /// <summary>
    /// Returns message about lists with different element counts.
    /// </summary>
    internal static bool DifferentCountInListsTU<T, U>(string firstListName, IList<T> firstList, string secondListName, IList<U> secondList)
    {
        return ThrowIsNotNull(
            Exceptions.DifferentCountInLists(FullNameOfExecutedCode(), firstListName, firstList.Count, secondListName, secondList.Count));
    }

    /// <summary>
    /// Returns message about not implemented case.
    /// </summary>
    internal static bool NotImplementedCase(object notImplementedName)
    { return ThrowIsNotNull(Exceptions.NotImplementedCase, notImplementedName); }
    /// <summary>
    /// Returns message about not implemented method.
    /// </summary>
    internal static bool NotImplementedMethod() { return ThrowIsNotNull(Exceptions.NotImplementedMethod); }

    /// <summary>
    /// Returns full name of the executed code.
    /// </summary>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type castedType)
        {
            typeFullName = castedType.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type actualType = type.GetType();
            typeFullName = actualType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    /// <summary>
    /// Throws the exception when the message is not null.
    /// </summary>
    internal static bool ThrowIsNotNull(string? exception, bool reallyThrow = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (reallyThrow)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }


    /// <summary>
    /// Throws the exception when the message is not null.
    /// </summary>
    internal static bool ThrowIsNotNull<A, B>(Func<string, A, B, string?> exceptionFunction, A firstArgument, B secondArgument)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode(), firstArgument, secondArgument);
        return ThrowIsNotNull(exception);
    }

    /// <summary>
    /// Throws the exception when the message is not null.
    /// </summary>
    internal static bool ThrowIsNotNull<A>(Func<string, A, string?> exceptionFunction, A argument)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode(), argument);
        return ThrowIsNotNull(exception);
    }

    /// <summary>
    /// Throws the exception when the message is not null.
    /// </summary>
    internal static bool ThrowIsNotNull(Func<string, string?> exceptionFunction)
    {
        string? exception = exceptionFunction(FullNameOfExecutedCode());
        return ThrowIsNotNull(exception);
    }
}