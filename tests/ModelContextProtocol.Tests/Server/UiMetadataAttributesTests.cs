using ModelContextProtocol.Server;
using System.Text.Json.Nodes;
using Xunit;

namespace ModelContextProtocol.Tests.Server;

public class UiMetadataAttributesTests
{
    #region UiTemplateAttribute Tests

    [Fact]
    public void UiTemplateAttribute_WithDefaultKey_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithUiTemplate))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("ui://widgets/task-board.html", tool.ProtocolTool.Meta["openai/outputTemplate"]?.GetValue<string>());
    }

    [Fact]
    public void UiTemplateAttribute_WithCustomKey_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithCustomTemplateKey))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("custom://widget.html", tool.ProtocolTool.Meta["custom/template"]?.GetValue<string>());
    }

    [Fact]
    public void UiTemplateAttribute_OnPrompt_PopulatesPromptMeta()
    {
        var method = typeof(TestPromptsWithUiMetadata).GetMethod(nameof(TestPromptsWithUiMetadata.PromptWithUiTemplate))!;
        
        var prompt = McpServerPrompt.Create(method, target: null);
        
        Assert.NotNull(prompt.ProtocolPrompt.Meta);
        Assert.Equal("ui://prompts/chat-template.html", prompt.ProtocolPrompt.Meta["openai/outputTemplate"]?.GetValue<string>());
    }

    [Fact]
    public void UiTemplateAttribute_OnResource_PopulatesResourceMeta()
    {
        var method = typeof(TestResourcesWithUiMetadata).GetMethod(nameof(TestResourcesWithUiMetadata.ResourceWithUiTemplate))!;
        
        var resource = McpServerResource.Create(method, target: null);
        
        Assert.NotNull(resource.ProtocolResourceTemplate?.Meta);
        Assert.Equal("ui://resources/viewer.html", resource.ProtocolResourceTemplate.Meta["openai/outputTemplate"]?.GetValue<string>());
    }

    #endregion

    #region LocaleAttribute Tests

    [Fact]
    public void LocaleAttribute_WithDefaultKey_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithLocale))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("en_US", tool.ProtocolTool.Meta["openai/locale"]?.GetValue<string>());
    }

    [Fact]
    public void LocaleAttribute_WithCustomKey_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithCustomLocaleKey))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("fr_FR", tool.ProtocolTool.Meta["custom/locale"]?.GetValue<string>());
    }

    [Fact]
    public void LocaleAttribute_OnPrompt_PopulatesPromptMeta()
    {
        var method = typeof(TestPromptsWithUiMetadata).GetMethod(nameof(TestPromptsWithUiMetadata.PromptWithLocale))!;
        
        var prompt = McpServerPrompt.Create(method, target: null);
        
        Assert.NotNull(prompt.ProtocolPrompt.Meta);
        Assert.Equal("ja_JP", prompt.ProtocolPrompt.Meta["openai/locale"]?.GetValue<string>());
    }

    [Fact]
    public void LocaleAttribute_OnResource_PopulatesResourceMeta()
    {
        var method = typeof(TestResourcesWithUiMetadata).GetMethod(nameof(TestResourcesWithUiMetadata.ResourceWithLocale))!;
        
        var resource = McpServerResource.Create(method, target: null);
        
        Assert.NotNull(resource.ProtocolResourceTemplate?.Meta);
        Assert.Equal("es_ES", resource.ProtocolResourceTemplate.Meta["openai/locale"]?.GetValue<string>());
    }

    #endregion

    #region UiHintAttribute Tests

    [Fact]
    public void UiHintAttribute_StringValue_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithStringHint))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("Data loaded successfully", tool.ProtocolTool.Meta["statusCopy"]?.GetValue<string>());
    }

    [Fact]
    public void UiHintAttribute_BoolValue_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithBoolHint))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.True(tool.ProtocolTool.Meta["readOnlyHint"]?.GetValue<bool>());
    }

    [Fact]
    public void UiHintAttribute_NumericValue_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithNumericHint))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal(0.75, tool.ProtocolTool.Meta["displayOpacity"]?.GetValue<double>());
    }

    [Fact]
    public void UiHintAttribute_MultipleHints_PopulatesToolMeta()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithMultipleHints))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("Panel loaded", tool.ProtocolTool.Meta["statusCopy"]?.GetValue<string>());
        Assert.False(tool.ProtocolTool.Meta["canInitiateToolCalls"]?.GetValue<bool>());
        Assert.Equal(1.0, tool.ProtocolTool.Meta["priority"]?.GetValue<double>());
    }

    [Fact]
    public void UiHintAttribute_OnPrompt_PopulatesPromptMeta()
    {
        var method = typeof(TestPromptsWithUiMetadata).GetMethod(nameof(TestPromptsWithUiMetadata.PromptWithUiHints))!;
        
        var prompt = McpServerPrompt.Create(method, target: null);
        
        Assert.NotNull(prompt.ProtocolPrompt.Meta);
        Assert.Equal("inline", prompt.ProtocolPrompt.Meta["displayMode"]?.GetValue<string>());
    }

    [Fact]
    public void UiHintAttribute_OnResource_PopulatesResourceMeta()
    {
        var method = typeof(TestResourcesWithUiMetadata).GetMethod(nameof(TestResourcesWithUiMetadata.ResourceWithUiHints))!;
        
        var resource = McpServerResource.Create(method, target: null);
        
        Assert.NotNull(resource.ProtocolResourceTemplate?.Meta);
        Assert.True(resource.ProtocolResourceTemplate.Meta["cacheable"]?.GetValue<bool>());
    }

    #endregion

    #region Combined Attributes Tests

    [Fact]
    public void CombinedAttributes_AllPopulateCorrectly()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithAllUiAttributes))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("ui://widgets/dashboard.html", tool.ProtocolTool.Meta["openai/outputTemplate"]?.GetValue<string>());
        Assert.Equal("en_US", tool.ProtocolTool.Meta["openai/locale"]?.GetValue<string>());
        Assert.Equal("Dashboard ready", tool.ProtocolTool.Meta["statusCopy"]?.GetValue<string>());
        Assert.False(tool.ProtocolTool.Meta["readOnlyHint"]?.GetValue<bool>());
    }

    [Fact]
    public void CombinedWithMcpMeta_AllPopulateCorrectly()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithUiAndMcpMeta))!;
        
        var tool = McpServerTool.Create(method, target: null);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("ui://widgets/panel.html", tool.ProtocolTool.Meta["openai/outputTemplate"]?.GetValue<string>());
        Assert.Equal("en_US", tool.ProtocolTool.Meta["openai/locale"]?.GetValue<string>());
        Assert.Equal("gpt-4o", tool.ProtocolTool.Meta["model"]?.GetValue<string>());
        Assert.Equal("1.0", tool.ProtocolTool.Meta["version"]?.GetValue<string>());
    }

    [Fact]
    public void OptionsMetaTakesPrecedence_OverUiAttributes()
    {
        var method = typeof(TestToolsWithUiMetadata).GetMethod(nameof(TestToolsWithUiMetadata.ToolWithUiTemplate))!;
        var options = new McpServerToolCreateOptions 
        { 
            Meta = new JsonObject 
            { 
                ["openai/outputTemplate"] = "override://template.html"
            } 
        };
        
        var tool = McpServerTool.Create(method, target: null, options: options);
        
        Assert.NotNull(tool.ProtocolTool.Meta);
        Assert.Equal("override://template.html", tool.ProtocolTool.Meta["openai/outputTemplate"]?.GetValue<string>());
    }

    #endregion

    #region Test Classes

    private class TestToolsWithUiMetadata
    {
        [McpServerTool]
        [UiTemplate("ui://widgets/task-board.html")]
        public static string ToolWithUiTemplate(string input) => input;

        [McpServerTool]
        [UiTemplate("custom://widget.html", "custom/template")]
        public static string ToolWithCustomTemplateKey(string input) => input;

        [McpServerTool]
        [Locale("en_US")]
        public static string ToolWithLocale(string input) => input;

        [McpServerTool]
        [Locale("fr_FR", "custom/locale")]
        public static string ToolWithCustomLocaleKey(string input) => input;

        [McpServerTool]
        [UiHint("statusCopy", "Data loaded successfully")]
        public static string ToolWithStringHint(string input) => input;

        [McpServerTool]
        [UiHint("readOnlyHint", true)]
        public static string ToolWithBoolHint(string input) => input;

        [McpServerTool]
        [UiHint("displayOpacity", 0.75)]
        public static string ToolWithNumericHint(string input) => input;

        [McpServerTool]
        [UiHint("statusCopy", "Panel loaded")]
        [UiHint("canInitiateToolCalls", false)]
        [UiHint("priority", 1.0)]
        public static string ToolWithMultipleHints(string input) => input;

        [McpServerTool]
        [UiTemplate("ui://widgets/dashboard.html")]
        [Locale("en_US")]
        [UiHint("statusCopy", "Dashboard ready")]
        [UiHint("readOnlyHint", false)]
        public static string ToolWithAllUiAttributes(string input) => input;

        [McpServerTool]
        [UiTemplate("ui://widgets/panel.html")]
        [Locale("en_US")]
        [McpMeta("model", "gpt-4o")]
        [McpMeta("version", "1.0")]
        public static string ToolWithUiAndMcpMeta(string input) => input;
    }

    private class TestPromptsWithUiMetadata
    {
        [McpServerPrompt]
        [UiTemplate("ui://prompts/chat-template.html")]
        public static string PromptWithUiTemplate(string input) => input;

        [McpServerPrompt]
        [Locale("ja_JP")]
        public static string PromptWithLocale(string input) => input;

        [McpServerPrompt]
        [UiHint("displayMode", "inline")]
        public static string PromptWithUiHints(string input) => input;
    }

    private class TestResourcesWithUiMetadata
    {
        [McpServerResource(UriTemplate = "resource://test/{id}")]
        [UiTemplate("ui://resources/viewer.html")]
        public static string ResourceWithUiTemplate(string id) => id;

        [McpServerResource(UriTemplate = "resource://test/{id}")]
        [Locale("es_ES")]
        public static string ResourceWithLocale(string id) => id;

        [McpServerResource(UriTemplate = "resource://test/{id}")]
        [UiHint("cacheable", true)]
        public static string ResourceWithUiHints(string id) => id;
    }

    #endregion
}
