using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Hangman;
using Hangman.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<GameState>();

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
