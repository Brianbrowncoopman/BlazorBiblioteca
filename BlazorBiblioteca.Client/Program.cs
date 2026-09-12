using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp =>
new HttpClient
{
    // BaseAddress = new Uri(builder.Configuration["FrontendURL"] ?? "https://localhost:7150")
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});


builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();

