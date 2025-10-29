# UI Metadata Support for ChatGPT and Custom UX

This document demonstrates how to use the MCP C# SDK's metadata attributes to build custom UI experiences, particularly for integration with ChatGPT's Apps SDK and other systems that support UI rendering via MCP.

## Overview

The MCP C# SDK provides several ways to attach metadata to tools, prompts, and resources:

1. **`McpMetaAttribute`** - Generic attribute for arbitrary metadata key/value pairs
2. **`UiTemplateAttribute`** - Convenience attribute for specifying UI template URIs
3. **`LocaleAttribute`** - Convenience attribute for internationalization support
4. **`UiHintAttribute`** - Convenience attribute for UI rendering hints

These attributes are provider-agnostic but include defaults optimized for ChatGPT's Apps SDK.

## Basic Usage

### UI Templates

Use `UiTemplateAttribute` to specify a custom UI template for rendering tool responses:

```csharp
using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerTool]
[UiTemplate("ui://widgets/task-board.html")]
[Description("Manages tasks with a custom task board UI")]
public static async Task<string> ManageTaskBoard(
    string action,
    string? taskId = null,
    CancellationToken cancellationToken = default)
{
    // Tool implementation
    return $"Task board updated: {action}";
}
```

By default, this sets the `openai/outputTemplate` metadata key. For other systems, specify a custom key:

```csharp
[UiTemplate("custom://widget.html", "myProvider/template")]
```

### Localization

Use `LocaleAttribute` to indicate the language of your response:

```csharp
[McpServerTool]
[Locale("en_US")]
[Description("Returns weather information in English")]
public static async Task<string> GetWeatherEnglish(
    string city,
    CancellationToken cancellationToken = default)
{
    return $"Weather in {city}: Sunny, 72°F";
}

[McpServerTool]
[Locale("fr_FR")]
[Description("Retourne les informations météo en français")]
public static async Task<string> GetWeatherFrench(
    string city,
    CancellationToken cancellationToken = default)
{
    return $"Météo à {city}: Ensoleillé, 22°C";
}
```

### UI Hints

Use `UiHintAttribute` to provide hints about UI behavior:

```csharp
[McpServerTool]
[UiHint("readOnlyHint", true)]
[UiHint("statusCopy", "Data loaded successfully")]
[UiHint("canInitiateToolCalls", false)]
[Description("Displays read-only data")]
public static string ViewData(string id)
{
    return $"Data for ID: {id}";
}
```

## Complete ChatGPT Integration Example

Here's a complete example of a tool designed for ChatGPT's Apps SDK:

```csharp
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

public class ChatGptTools
{
    [McpServerTool]
    [UiTemplate("ui://widgets/kanban-board.html")]
    [Locale("en_US")]
    [UiHint("statusCopy", "Board loaded successfully")]
    [UiHint("canInitiateToolCalls", true)]
    [Description("Manages a kanban board with custom UI")]
    public static async Task<string> ManageKanbanBoard(
        [Description("Action: add, move, delete")] string action,
        [Description("Task title")] string? title = null,
        [Description("Column: todo, doing, done")] string? column = null,
        CancellationToken cancellationToken = default)
    {
        // Your implementation here
        var result = new
        {
            action,
            title,
            column,
            timestamp = DateTime.UtcNow
        };
        
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool]
    [UiTemplate("ui://widgets/calendar-view.html")]
    [Locale("en_US")]
    [UiHint("readOnlyHint", false)]
    [UiHint("displayMode", "modal")]
    [Description("Displays and manages calendar events")]
    public static async Task<string> ManageCalendar(
        [Description("View type: day, week, month")] string view,
        [Description("Selected date in ISO format")] string? date = null,
        CancellationToken cancellationToken = default)
    {
        // Your implementation here
        return JsonSerializer.Serialize(new { view, date });
    }
}
```

## Using with Prompts

Metadata attributes work with prompts too:

```csharp
[McpServerPrompt]
[UiTemplate("ui://prompts/interactive-form.html")]
[Locale("en_US")]
[Description("Interactive prompt with custom form UI")]
public static ChatMessage InteractivePrompt(
    [Description("Form type")] string formType)
{
    return new ChatMessage(
        ChatRole.User, 
        $"Please fill out the {formType} form"
    );
}
```

## Using with Resources

And with resources:

```csharp
[McpServerResource(UriTemplate = "document://{id}")]
[UiTemplate("ui://viewers/document-viewer.html")]
[Locale("en_US")]
[UiHint("cacheable", true)]
[UiHint("previewEnabled", true)]
[Description("Document resource with custom viewer")]
public static async Task<string> GetDocument(
    string id,
    CancellationToken cancellationToken = default)
{
    // Return document content
    return $"Document content for {id}";
}
```

## Combining with Generic Metadata

You can combine convenience attributes with `McpMetaAttribute` for additional metadata:

```csharp
[McpServerTool]
[UiTemplate("ui://widgets/dashboard.html")]
[Locale("en_US")]
[McpMeta("model", "gpt-4o")]
[McpMeta("version", "2.0")]
[McpMeta("features", JsonValue = """["real-time", "interactive", "responsive"]""")]
[Description("Interactive dashboard with real-time updates")]
public static async Task<string> Dashboard(
    CancellationToken cancellationToken = default)
{
    // Dashboard implementation
    return "Dashboard data";
}
```

## ChatGPT Apps SDK Integration

When using with ChatGPT's Apps SDK:

1. **Register your UI templates** in your MCP server
2. **Serve the HTML/JavaScript** for your widgets
3. **Use the `openai/outputTemplate` metadata** to reference your templates
4. **Include `openai/locale`** for proper internationalization
5. **Use UI hints** like `canInitiateToolCalls`, `readOnlyHint`, and `statusCopy` to control behavior

Example server setup:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register MCP server with tools
builder.Services
    .AddMcpServer()
    .WithTools<ChatGptTools>();

var app = builder.Build();

// Serve UI templates (if hosting them)
app.UseStaticFiles();

// Map MCP endpoints
app.MapMcp("/mcp");

app.Run();
```

## Provider-Agnostic Usage

While these attributes default to ChatGPT-compatible keys, they're designed to work with any system:

```csharp
// Custom provider metadata
[UiTemplate("myapp://widgets/custom.html", "myProvider/template")]
[Locale("en_US", "myProvider/lang")]
public static string MyCustomTool(string input) => input;
```

## Metadata Priority

When metadata is specified in multiple places, the priority is:

1. **Options object** (`McpServerToolCreateOptions.Meta`)
2. **Convenience attributes** (`UiTemplate`, `Locale`, `UiHint`)
3. **Generic attributes** (`McpMeta`)

Example:

```csharp
var options = new McpServerToolCreateOptions 
{ 
    Meta = new JsonObject 
    { 
        ["openai/outputTemplate"] = "override://template.html" // Takes precedence
    } 
};

var tool = McpServerTool.Create(
    method,
    target: null,
    options: options
);
```

## Best Practices

1. **Use convenience attributes** for common scenarios (templates, locales, hints)
2. **Use `McpMeta`** for custom or complex metadata
3. **Keep metadata provider-agnostic** when possible
4. **Document your metadata keys** if using custom providers
5. **Test with actual clients** to ensure proper rendering
6. **Version your UI templates** to prevent caching issues

## Additional Resources

- [OpenAI Apps SDK Documentation](https://developers.openai.com/apps-sdk)
- [MCP Specification](https://spec.modelcontextprotocol.io/)
- [Example ChatGPT Apps](https://github.com/openai/openai-apps-sdk-examples)
