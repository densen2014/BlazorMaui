using SpiderEye;

namespace testsp.Core
{
    public abstract class ProgramBase
    {
        protected static void Run()
        {
            using (var window = new Window())
            {
                window.Title = "testsp";
                window.Icon = AppIcon.FromFile("icon", ".");

                Application.ContentProvider = new EmbeddedContentProvider("App");
                Application.Run(window, "index.html");
            }
        }
    }
}
