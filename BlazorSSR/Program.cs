// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes =
    ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "image/svg+xml" });
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = (CompressionLevel)4;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSharedExtensions();
builder.Services.AddFileSystemExtensions();
builder.Services.AddOcrExtensions(builder.Configuration["AzureCvKey"], builder.Configuration["AzureCvUrl"]);
builder.Services.AddAIFormExtensions(builder.Configuration["AzureAiFormKey"], builder.Configuration["AzureAiFormUrl"]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseStaticFiles();
//app.UseDefaultFiles();
app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    RequestPath = new PathString("/pic")
});
app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
