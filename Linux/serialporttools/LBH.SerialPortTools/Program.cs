using Gtk;
using LBH.SerialPortTools;

class Program
{
    public static Application App;
    public static MainWindow mainWindow;
    [STAThread]
    public static void Main(string[] args)
    {
        Application.Init();

        App = new Application("LBH.SerialPortTools", GLib.ApplicationFlags.None);
        App.Register(GLib.Cancellable.Current);

        Gtk.CssProvider provider = new Gtk.CssProvider();
        provider.LoadFromPath("sp.css");
        Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, 800);

        mainWindow = new MainWindow();
        mainWindow.SetIconFromFile("logo.png");
        App.AddWindow(mainWindow);
        
        Application.Run();
    }
}