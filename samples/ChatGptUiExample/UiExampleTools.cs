using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace ChatGptUiExample;

/// <summary>
/// Example tools demonstrating UI metadata attributes for ChatGPT Apps SDK integration.
/// </summary>
[McpServerToolType]
public class UiExampleTools
{
    /// <summary>
    /// Example: Task board with custom UI template and metadata.
    /// </summary>
    [McpServerTool]
    [UiTemplate("ui://widgets/task-board.html")]
    [Locale("en_US")]
    [UiHint("statusCopy", "Task board ready")]
    [UiHint("canInitiateToolCalls", true)]
    [Description("Manages a task board with drag-and-drop UI")]
    public static string ManageTaskBoard(
        [Description("Action to perform: list, add, move, delete")] string action,
        [Description("Task title (for add action)")] string? title = null,
        [Description("Task ID (for move/delete actions)")] string? taskId = null,
        [Description("Target column: todo, in-progress, done")] string? column = null)
    {
        var result = new
        {
            action,
            taskId,
            title,
            column,
            timestamp = DateTime.UtcNow.ToString("o"),
            tasks = action == "list" ? new[]
            {
                new { id = "1", title = "Example Task 1", column = "todo" },
                new { id = "2", title = "Example Task 2", column = "in-progress" }
            } : null
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Example: Calendar view with custom UI and interaction hints.
    /// </summary>
    [McpServerTool]
    [UiTemplate("ui://widgets/calendar.html")]
    [Locale("en_US")]
    [UiHint("readOnlyHint", false)]
    [UiHint("displayMode", "modal")]
    [McpMeta("model", "gpt-4o")]
    [Description("Interactive calendar with event management")]
    public static string ViewCalendar(
        [Description("View type: day, week, month, year")] string view = "month",
        [Description("Date to display (ISO 8601 format)")] string? date = null)
    {
        var displayDate = string.IsNullOrEmpty(date) 
            ? DateTime.Today 
            : DateTime.Parse(date);

        var result = new
        {
            view,
            date = displayDate.ToString("yyyy-MM-dd"),
            events = new[]
            {
                new { 
                    id = "evt-1", 
                    title = "Team Meeting", 
                    start = displayDate.AddHours(10).ToString("o"),
                    end = displayDate.AddHours(11).ToString("o")
                },
                new { 
                    id = "evt-2", 
                    title = "Project Review", 
                    start = displayDate.AddHours(14).ToString("o"),
                    end = displayDate.AddHours(15).ToString("o")
                }
            }
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Example: Data visualization with read-only hint.
    /// </summary>
    [McpServerTool]
    [UiTemplate("ui://widgets/chart-dashboard.html")]
    [Locale("en_US")]
    [UiHint("readOnlyHint", true)]
    [UiHint("statusCopy", "Dashboard loaded")]
    [Description("Displays analytics dashboard with charts")]
    public static string AnalyticsDashboard(
        [Description("Time range: day, week, month, year")] string range = "week")
    {
        var result = new
        {
            range,
            metrics = new
            {
                totalUsers = 15420,
                activeUsers = 3891,
                revenue = 47239.50,
                conversion = 3.2
            },
            chartData = new[]
            {
                new { date = "2024-01-15", users = 1200, revenue = 4500 },
                new { date = "2024-01-16", users = 1350, revenue = 5200 },
                new { date = "2024-01-17", users = 1180, revenue = 4300 },
                new { date = "2024-01-18", users = 1420, revenue = 5800 },
                new { date = "2024-01-19", users = 1510, revenue = 6100 }
            }
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Example: Multi-language support with locale attribute.
    /// </summary>
    [McpServerTool]
    [Locale("fr_FR")]
    [Description("Retourne les informations météo en français")]
    public static string GetWeatherFrench(
        [Description("Nom de la ville")] string city)
    {
        return $"Météo à {city}: Ensoleillé, 22°C, Humidité: 65%";
    }

    /// <summary>
    /// Example: Form with multiple UI hints.
    /// </summary>
    [McpServerTool]
    [UiTemplate("ui://widgets/feedback-form.html")]
    [Locale("en_US")]
    [UiHint("statusCopy", "Form ready for input")]
    [UiHint("canInitiateToolCalls", false)]
    [UiHint("requiresInput", true)]
    [Description("Displays an interactive feedback form")]
    public static string FeedbackForm(
        [Description("Form type: bug, feature, general")] string type = "general")
    {
        var result = new
        {
            type,
            fields = new object[]
            {
                new { name = "subject", label = "Subject", type = "text", required = true },
                new { name = "description", label = "Description", type = "textarea", required = true },
                new { name = "priority", label = "Priority", type = "select", options = new[] { "low", "medium", "high" } }
            }
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
}

/// <summary>
/// Example prompts with UI metadata.
/// </summary>
[McpServerPromptType]
public class UiExamplePrompts
{
    /// <summary>
    /// Interactive prompt with custom template.
    /// </summary>
    [McpServerPrompt]
    [UiTemplate("ui://prompts/wizard.html")]
    [Locale("en_US")]
    [Description("Step-by-step wizard prompt")]
    public static string WizardPrompt(
        [Description("Wizard type: setup, tutorial, onboarding")] string type)
    {
        return $"Welcome to the {type} wizard. Let's get started!";
    }
}

/// <summary>
/// Example resources with UI metadata.
/// </summary>
[McpServerResourceType]
public class UiExampleResources
{
    /// <summary>
    /// Document viewer with custom UI.
    /// </summary>
    [McpServerResource(UriTemplate = "document://{id}")]
    [UiTemplate("ui://viewers/document.html")]
    [Locale("en_US")]
    [UiHint("cacheable", true)]
    [UiHint("previewEnabled", true)]
    [Description("Document with rich text viewer")]
    public static string ViewDocument(string id)
    {
        var result = new
        {
            id,
            title = $"Document {id}",
            content = "This is the document content with **markdown** support.",
            metadata = new
            {
                author = "John Doe",
                created = DateTime.UtcNow.AddDays(-7).ToString("o"),
                modified = DateTime.UtcNow.ToString("o")
            }
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
}
