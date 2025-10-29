using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChatGptUiExample;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<UiExampleTools>()
    .WithPrompts<UiExamplePrompts>()
    .WithResources<UiExampleResources>();

await builder.Build().RunAsync();
