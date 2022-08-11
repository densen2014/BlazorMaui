using LibraryShared;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using static Microsoft.Maui.ApplicationModel.Permissions;
#if ANDROID
using AndroidX.Activity;
#elif WINDOWS
using BlazorMaui.Platforms.Windows;
#endif

namespace BlazorMaui
{
    public partial class SensorsPage : ContentPage
    {
        public SensorsPage()
        {
            InitializeComponent();
              
        }
         
    }
}
