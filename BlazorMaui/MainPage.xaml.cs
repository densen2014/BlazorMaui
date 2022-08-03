using LibraryShared;
using Microsoft.Maui.Controls;
using System;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace BlazorMaui
{
    public partial class MainPage : ContentPage
    {
        TestService TestService=new TestService();
        public MainPage()
        {
            InitializeComponent();
        }

        private void BlazorWebView_BlazorWebViewInitialized(object s, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
        {
            TestService.BlazorWebView_BlazorWebViewInitialized(s, e);
        }

    }
}
