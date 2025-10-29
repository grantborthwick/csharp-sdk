using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.Server;

/// <summary>
/// Provides a hint for UI rendering or interaction behavior.
/// </summary>
/// <remarks>
/// <para>
/// This attribute is a convenience wrapper around <see cref="McpMetaAttribute"/> that sets
/// UI-related hints for client systems. It can be used to control various aspects of how
/// the tool, prompt, or resource is presented or behaves in a user interface.
/// </para>
/// <para>
/// Common hint keys include:
/// <list type="bullet">
///   <item>"readOnlyHint" - Indicates whether the data is read-only</item>
///   <item>"statusCopy" - Provides custom status text</item>
///   <item>"canInitiateToolCalls" - Controls whether UI can trigger tool calls</item>
///   <item>"displayMode" - Suggests how the UI should be displayed</item>
/// </list>
/// </para>
/// <para>
/// Multiple hints can be provided by applying this attribute multiple times with different keys.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [McpServerTool]
/// [UiHint("readOnlyHint", true)]
/// [UiHint("statusCopy", "Data loaded successfully")]
/// [Description("Displays read-only data")]
/// public static string ViewData(string id)
/// {
///     // Tool implementation
///     return $"Data for {id}";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class UiHintAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UiHintAttribute"/> class with a string value.
    /// </summary>
    /// <param name="key">The hint key.</param>
    /// <param name="value">The string value of the hint.</param>
    public UiHintAttribute(string key, string value)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value;
        ValueType = UiHintValueType.String;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UiHintAttribute"/> class with a boolean value.
    /// </summary>
    /// <param name="key">The hint key.</param>
    /// <param name="value">The boolean value of the hint.</param>
    public UiHintAttribute(string key, bool value)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        BoolValue = value;
        ValueType = UiHintValueType.Boolean;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UiHintAttribute"/> class with a numeric value.
    /// </summary>
    /// <param name="key">The hint key.</param>
    /// <param name="value">The numeric value of the hint.</param>
    public UiHintAttribute(string key, double value)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        NumericValue = value;
        ValueType = UiHintValueType.Numeric;
    }

    /// <summary>
    /// Gets the hint key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the string value of the hint (when applicable).
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Gets the boolean value of the hint (when applicable).
    /// </summary>
    public bool BoolValue { get; }

    /// <summary>
    /// Gets the numeric value of the hint (when applicable).
    /// </summary>
    public double NumericValue { get; }

    /// <summary>
    /// Gets the type of value stored in this hint.
    /// </summary>
    internal UiHintValueType ValueType { get; }

    internal enum UiHintValueType
    {
        String,
        Boolean,
        Numeric
    }
}
