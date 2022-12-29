// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using SpiderEye.Linux;
using SpiderEye;
using System.Diagnostics;

LinuxApplication.Init();

using (var window = new SpiderEye.Window())
{
    // window.Icon = AppIcon.FromFile("icon", ".");

    // these are only called in Debug mode:
    DevSettings.SetDevSettings(window);

    // the port number is defined in the angular.json file (under "architect"->"serve"->"options"->"port")
    // note that you have to run the angular dev server first (npm run watch)
    //SpiderEye.Application.UriWatcher = new AngularDevUriWatcher("http://blazor.app1.es");

    // this relates to the path defined in the .csproj file
    //SpiderEye.Application.ContentProvider = new EmbeddedContentProvider("Angular\\dist");

    // runs the application and opens the window with the given page loaded
    SpiderEye.Application.Run(window, "http://blazor.zone");
    //SpiderEye.Application.Run(window, "/index.html");
}
public class AngularDevUriWatcher : IUriWatcher
{
    private readonly Uri devServerUri;

    public AngularDevUriWatcher(string devServerUri)
    {
        this.devServerUri = new Uri(devServerUri);
    }

    public Uri CheckUri(Uri uri)
    {
        // this is only called in debug mode
        CheckDevUri(ref uri);

        return uri;
    }

    [Conditional("DEBUG")]
    private void CheckDevUri(ref Uri uri)
    {
        // this changes a relative URI (e.g. /index.html) to
        // an absolute URI with the Angular dev server as host
        if (!uri.IsAbsoluteUri)
        {
            uri = new Uri(devServerUri, uri);
        }
    }
}

public class DevSettings
{
    [Conditional("DEBUG")]
    public static void SetDevSettings(SpiderEye.Window window)
    {
        window.EnableDevTools = true;

        var stage = "1";

        // this is just to give some suggestions in case something isn't set up correctly for development
        window.PageLoaded +=   (s, e) =>
        {
            if (!e.Success)
            {
                string message = $"Page did not load!{Environment.NewLine}Did you start the Angular dev server?";
                if (SpiderEye.Application.OS == SpiderEye.OperatingSystem.Windows)
                {
                    message += $"{Environment.NewLine}On Windows 10 you also have to allow localhost. More info can be found in the SpiderEye readme.";
                }

                SpiderEye.MessageBox.Show(window, message, "Page load failed", MessageBoxButtons.Ok);
            }

            if (stage == "1")
                //await window.Bridge.InvokeAsync("11",$@"
                //            console.log(""Hello World"");
                //            document.querySelectorAll('input[title=""Search""]')[0].value = ""Hello World"";
                //            document.querySelectorAll('input[value=""Google Search""]')[0].click();");

            stage = "2";
        };
    }
}

