// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorShared;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using SpiderEye;
using Application = SpiderEye.Application;
using OperatingSystem = SpiderEye.OperatingSystem;
using BlazorShared.Services;
#if WINDOWS
using SpiderEye.Windows;
#else
using SpiderEye.Linux;
using SpiderEye.Mac;
#endif

internal class Program
{

    #region SSR host
    private static void MainSsr(string[] args)
    {

        AppState _appState = new();
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
            options.Level = CompressionLevel.Optimal;
        });

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSharedExtensions();
        builder.Services.AddOcrExtensions();
        builder.Services.AddAIFormExtensions();
        builder.Services.AddSingleton(_appState);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            //app.UseHsts();
        }
        app.UseResponseCompression();

        //app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseDirectoryBrowser(new DirectoryBrowserOptions()
        {
            RequestPath = new PathString("/pic")
        });
        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.RunAsync();
    }
    #endregion

    [STAThread]
    public static void Main(string[] args)
    {
        MainSsr(args);

        #region CrossUI

#if WINDOWS
        WindowsApplication.Init();
#else
        if (Application.OS == OperatingSystem.Linux)
        {
            LinuxApplication.Init();
        }
        else if (Application.OS == OperatingSystem.MacOS)
        {
            MacApplication.Init();
        }
#endif
        var icon = AppIcon.FromFile("icon", AppDomain.CurrentDomain.BaseDirectory);

#if WINDOWS
        using var statusIcon = new StatusIcon();
#endif
        try
        {
            using var window = new Window();
            window.Title = "BlazorLinux (Linux/Win/Mac)";
            window.UseBrowserTitle = true;
            window.EnableScriptInterface = true;
            window.CanResize = true;
            window.BackgroundColor = "#303030";
            window.Icon = icon;

#if DEBUG
            window.EnableDevTools = true;
#endif

            var bridge = new CrossBridgeService();
            Application.AddGlobalHandler(bridge);
            Application.Run(window, "http://localhost:5000");
        }
        catch (Exception e)
        {
            if (e.HResult == -2146233079 && Application.OS == OperatingSystem.Windows)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\n\n Windows 环境请使用 net7.0-windows10.0.17763 环境执行.\n\nIn windows please use target framework with 'net7.0-windows10.0.17763'. \n\n\n\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else {
                throw;
            }
           
        }
        #endregion

    }

}

