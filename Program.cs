using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Hangman;
using Hangman.Components;
using Hangman.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient as singleton
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register singleton services
builder.Services.AddSingleton<GameState>();
builder.Services.AddSingleton<WordCategoryService>();
builder.Services.AddSingleton<ThemeService>();

// Configure basic logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Build the WebAssembly host
var host = builder.Build();

try
{
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Application start-up failed: {ex.Message}");
}
