namespace SunamoCSharp;

/// <summary>
/// Represents the c sharp generator type.
/// </summary>
public partial class CSharpGenerator : GeneratorCodeAbstract //, ICSharpGenerator
{
    /// <summary>
    /// Start class.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="isStatic">The is static.</param>
    /// <param name="className">The class name.</param>
    /// <param name="derivedTypes">The derived types.</param>
    public void StartClass(int tabCount, AccessModifiers accessModifier, bool isStatic, string className, params string[] derivedTypes)
    {
        AddTab(tabCount);
        PublicStatic(accessModifier, isStatic);
        sb.AddItem(" class " + className);
        if (derivedTypes.Length != 0)
        {
            sb.AddItem(":");
            for (var i = 0; i < derivedTypes.Length - 1; i++)
                sb.AddItem(derivedTypes[i] + ",");
            sb.AddItem(derivedTypes[derivedTypes.Length - 1]);
        }

        StartBrace(tabCount);
    }

    private void PublicStatic(AccessModifiers accessModifier, bool isStatic)
    {
        WriteAccessModifiers(accessModifier);
        if (isStatic)
            sb.AddItem("static");
    }

    private void WriteAccessModifiers(AccessModifiers accessModifier)
    {
        if (accessModifier == AccessModifiers.Public)
        {
            sb.AddItem("public");
        }
        else if (accessModifier == AccessModifiers.Protected)
        {
            sb.AddItem("protected");
        }
        else if (accessModifier == AccessModifiers.Private)
        {
            // Private is default - no keyword needed
        }
        else if (accessModifier == AccessModifiers.Internal)
        {
            sb.AddItem("public");
        }
        else
        {
            ThrowEx.NotImplementedCase(accessModifier);
        }
    }

    /// <summary>
    /// End region.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    public void EndRegion(int tabCount)
    {
        AppendLine(tabCount, "#endregion");
    }

    /// <summary>
    /// Region.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="regionName">The region name.</param>
    public void Region(int tabCount, string regionName)
    {
        AppendLine(tabCount, "#region " + regionName);
    }

    /// <summary>
    /// Attribute.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="attributeParameters">The attribute parameters.</param>
    public void Attribute(int tabCount, string attributeName, string attributeParameters)
    {
        AddTab(tabCount);
        sb.AppendLine("[" + attributeName + "(" + attributeParameters + ")]");
    }

    /// <summary>
    /// Create enum items from list.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <returns>The create enum items from list result.</returns>
    public static List<EnumItem> CreateEnumItemsFromList(List<string> list)
    {
        var enumItems = new List<EnumItem>(list.Count);
        foreach (var item in list)
            enumItems.Add(new EnumItem { Name = item });
        return enumItems;
    }

    /// <summary>
    /// Field.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="isStatic">The is static.</param>
    /// <param name="variableModifiers">The variable modifiers.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="isAddingHyphensToValue">The is adding hyphens to value.</param>
    /// <param name="value">The value.</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, bool isAddingHyphensToValue, string value)
    {
        var initializationOption = ObjectInitializationOptions.Original;
        if (isAddingHyphensToValue)
            initializationOption = ObjectInitializationOptions.Hyphens;
        Field(tabCount, accessModifier, isStatic, variableModifiers, type, name, initializationOption, value);
    }

    /// <summary>
    /// Field.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="isStatic">The is static.</param>
    /// <param name="variableModifiers">The variable modifiers.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="initializationOption">The initialization option.</param>
    /// <param name="value">The value.</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, ObjectInitializationOptions initializationOption, string value)
    {
        AddTab(tabCount);
        ModifiersField(accessModifier, isStatic, variableModifiers);
        ReturnTypeName(type, name);
        sb.AddItem("=");
        if (initializationOption == ObjectInitializationOptions.Hyphens)
            value = "\"" + value + "\"";
        else if (initializationOption == ObjectInitializationOptions.NewAssign)
            value = "new " + type + "()";
        var statement = value + ";";
        sb.AddItem(statement);
        sb.AppendLine();
    }

    /// <summary>
    /// Field.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="isStatic">The is static.</param>
    /// <param name="variableModifiers">The variable modifiers.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="isUsingDefaultValue">The is using default value.</param>
    public void Field(int tabCount, AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers, string type, string name, bool isUsingDefaultValue)
    {
        AddTab(tabCount);
        ModifiersField(accessModifier, isStatic, variableModifiers);
        ReturnTypeName(type, name);
        DefaultValue(type, isUsingDefaultValue);
        sb.RemoveEndDelimiter();
        sb.AddItem(";");
        sb.AppendLine();
    }

    private void DefaultValue(string type, bool isUsingDefaultValue)
    {
        if (isUsingDefaultValue)
        {
            sb.AddItem("=");
            sb.AddItem(CSharpHelperSunamo.DefaultValueForType(type, ConvertTypeShortcutFullName.ToShortcut));
        }
    }

    /// <summary>
    /// Add into class.
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <param name="insertedLines">The inserted lines.</param>
    /// <param name="classIndex">The class index.</param>
    /// <param name="namespaceName">The namespace name.</param>
    /// <returns>The add into class result.</returns>
    public static List<string> AddIntoClass(List<string> lines, List<string> insertedLines, out int classIndex, string namespaceName)
    {
        classIndex = -1;
        var foundClass = false;
        var foundOpeningBrace = false;

        for (var i = 0; i < lines.Count; i++)
        {
            if (!foundClass)
            {
                if (lines[i].Contains(" class "))
                {
                    lines[i] = lines[i].Replace(namespaceName + "Page", "Page");
                    classIndex = i;
                    foundClass = true;
                }
            }
            else if (foundClass && !foundOpeningBrace)
            {
                if (lines[i].Contains("{"))
                    foundOpeningBrace = true;
            }
            else if (foundClass && foundOpeningBrace)
            {
                if (lines[i].Contains("}"))
                {
                    lines.InsertRange(i, insertedLines);
                    break;
                }
            }
        }

        return lines;
    }

    /// <summary>
    /// Namespace.
    /// </summary>
    /// <param name="namespaceName">The namespace name.</param>
    public void Namespace(string namespaceName)
    {
        sb.AddItem("namespace" + " " + namespaceName);
        sb.AppendLine();
        sb.AddItem("{");
        sb.AppendLine();
    }

    private void ModifiersField(AccessModifiers accessModifier, bool isStatic, VariableModifiers variableModifiers)
    {
        WriteAccessModifiers(accessModifier);
        if (variableModifiers == VariableModifiers.Mapped)
        {
            sb.AddItem("const");
        }
        else
        {
            if (isStatic && variableModifiers == VariableModifiers.ReadOnly)
            {
                sb.AddItem("const");
            }
            else
            {
                if (isStatic)
                    sb.AddItem("static");
                if (variableModifiers == VariableModifiers.ReadOnly)
                    sb.AddItem("readonly");
            }
        }
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="tabCount">The tab count.</param>
    /// <param name="modifierType">The modifier type.</param>
    /// <param name="constructorName">The constructor name.</param>
    /// <param name="bodyContent">The body content.</param>
    /// <param name="parameters">The parameters.</param>
    public void Ctor(int tabCount, ModifiersConstructor modifierType, string constructorName, string bodyContent, params string[] parameters)
    {
        AddTab(tabCount);
        var modifierStringBuilder = new StringBuilder(modifierType.ToString());
        modifierStringBuilder[0] = char.ToLower(modifierStringBuilder[0]);
        sb.AddItem(modifierStringBuilder.ToString());
        sb.AddItem(constructorName);
        StartParenthesis();
        var parameterNames = new List<string>(parameters.Length / 2);
        for (var i = 0; i < parameters.Length; i++)
        {
            sb.AddItem(parameters[i]);
            var parameterName = parameters[++i];
            parameterNames.Add(parameterName);
            if (i != parameters.Length - 1)
                sb.AddItem(parameterName + ",");
            else
                sb.AddItem(parameterName);
        }

        EndParenthesis();
        StartBrace(tabCount);
        Append(tabCount + 1, bodyContent);
        EndBrace(tabCount - 2);
        sb.AppendLine();
    }
}