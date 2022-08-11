using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;
using System;

namespace BlazorMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static void HandleAppActions(AppAction appAction)
        {
            //App.Current.Dispatcher.Dispatch(async () =>
            //{
            //    var page = appAction.Id switch
            //    {
            //        "battery_info" => new SensorsPage(),
            //        "app_info" => new AppModelPage(),
            //        _ => default(MainPage)
            //    };

            //    if (page != null)
            //    {
            //        await Application.Current.MainPage.Navigation.PopToRootAsync();
            //        await Application.Current.MainPage.Navigation.PushAsync(page);
            //    }
            //});
        }
    }
}
