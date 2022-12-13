using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorShared;
using Microsoft.Extensions.Configuration;

AppState _appState = new();
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddSharedExtensions();
builder.Configuration.AddUserSecrets<ConfigFake>();
builder.Services.AddOcrExtensions(builder.Configuration["AzureCvKey"], builder.Configuration["AzureCvUrl"]);
builder.Services.AddAIFormExtensions(builder.Configuration["AzureAiFormKey"], builder.Configuration["AzureAiFormUrl"]);
builder.Services.AddSingleton(_appState);
await builder.Build().RunAsync();
