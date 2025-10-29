using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.Server;

/// <summary>
/// Specifies a UI template URI for rendering custom user interface elements.
/// </summary>
/// <remarks>
/// <para>
/// This attribute is a convenience wrapper around <see cref="McpMetaAttribute"/> that sets
/// metadata for UI template rendering. It's commonly used with systems like ChatGPT's Apps SDK
/// to specify HTML/JavaScript templates that should be rendered inline with responses.
/// </para>
/// <para>
/// The template URI can be:
/// <list type="bullet">
///   <item>A custom URI scheme (e.g., "ui://widget/kanban-board.html")</item>
///   <item>An HTTP/HTTPS URL pointing to a hosted template</item>
///   <item>A resource URI registered with your MCP server</item>
/// </list>
/// </para>
/// <para>
/// This attribute can be combined with other metadata attributes like <see cref="McpMetaAttribute"/>
/// to provide additional context for UI rendering.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [McpServerTool]
/// [UiTemplate("ui://widgets/task-list.html")]
/// [Description("Manages a task list with custom UI")]
/// public static async Task&lt;string&gt; ManageTaskList(string action, CancellationToken cancellationToken)
/// {
///     // Tool implementation
///     return "Task list updated";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class UiTemplateAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UiTemplateAttribute"/> class.
    /// </summary>
    /// <param name="templateUri">The URI of the UI template to use for rendering.</param>
    /// <param name="metadataKey">
    /// The metadata key to use. Defaults to "openai/outputTemplate" for ChatGPT compatibility.
    /// Can be customized for other systems.
    /// </param>
    public UiTemplateAttribute(string templateUri, string metadataKey = "openai/outputTemplate")
    {
        TemplateUri = templateUri ?? throw new ArgumentNullException(nameof(templateUri));
        MetadataKey = metadataKey ?? throw new ArgumentNullException(nameof(metadataKey));
    }

    /// <summary>
    /// Gets the URI of the UI template.
    /// </summary>
    public string TemplateUri { get; }

    /// <summary>
    /// Gets the metadata key used to store the template URI.
    /// </summary>
    /// <remarks>
    /// Defaults to "openai/outputTemplate" for ChatGPT compatibility, but can be customized
    /// for other systems that support UI templates via metadata.
    /// </remarks>
    public string MetadataKey { get; }
}
