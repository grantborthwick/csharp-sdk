# ChatGPT UI Example

This sample demonstrates how to use UI metadata attributes with the MCP C# SDK to create tools, prompts, and resources that integrate with ChatGPT's Apps SDK and other UI-enabled MCP clients.

## Features

This example showcases:

- **`UiTemplateAttribute`** - Specifying custom UI templates for rich rendering
- **`LocaleAttribute`** - Internationalization support
- **`UiHintAttribute`** - UI behavior hints (read-only, status messages, interaction controls)
- **`McpMetaAttribute`** - Additional custom metadata
- Combined usage of multiple metadata attributes

## Example Tools

1. **ManageTaskBoard** - Task management with drag-and-drop UI
2. **ViewCalendar** - Interactive calendar with event management
3. **AnalyticsDashboard** - Read-only analytics dashboard with charts
4. **GetWeatherFrench** - Multi-language support example
5. **FeedbackForm** - Interactive form with UI hints

## Running the Sample

```bash
dotnet run
```

The server will start listening on stdio and display available tools with their metadata.

## Integration with ChatGPT

To use this with ChatGPT's Apps SDK:

1. Register the server URL in ChatGPT Developer Mode
2. Implement the UI templates referenced in the metadata (e.g., `ui://widgets/task-board.html`)
3. The templates will receive the tool output and render custom UI

## Metadata Explanation

Each tool demonstrates different metadata patterns:

### Task Board Tool
```csharp
[UiTemplate("ui://widgets/task-board.html")]  // Custom UI template
[Locale("en_US")]                              // Language/locale
[UiHint("statusCopy", "Task board ready")]     // Status message
[UiHint("canInitiateToolCalls", true)]         // Allows UI to trigger tools
```

### Analytics Dashboard Tool
```csharp
[UiTemplate("ui://widgets/chart-dashboard.html")]  // Chart UI template
[UiHint("readOnlyHint", true)]                     // Data is read-only
[UiHint("statusCopy", "Dashboard loaded")]         // Status message
```

### French Weather Tool
```csharp
[Locale("fr_FR")]  // French locale for i18n
```

## Provider-Agnostic Design

While the default metadata keys are optimized for ChatGPT (`openai/outputTemplate`, `openai/locale`), you can customize them for other systems:

```csharp
[UiTemplate("custom://widget.html", "myProvider/template")]
[Locale("en_US", "myProvider/lang")]
```

## See Also

- [UI Metadata Documentation](../../docs/UI_METADATA.md)
- [OpenAI Apps SDK](https://developers.openai.com/apps-sdk)
- [MCP Specification](https://spec.modelcontextprotocol.io/)
