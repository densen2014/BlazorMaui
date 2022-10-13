using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorShared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddSharedExtensions();
builder.Services.AddOcrExtensions(builder.Configuration["AzureCvKey"], builder.Configuration["AzureCvUrl"]);
builder.Services.AddAIFormExtensions(builder.Configuration["AzureAiFormKey"], builder.Configuration["AzureAiFormUrl"]);
await builder.Build().RunAsync();
