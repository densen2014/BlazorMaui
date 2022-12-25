// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using System.Reflection;

namespace MauiApp7test.Pages;

public partial class Index
{
 

 
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
#if WINDOWS
            WinUI.App.SetTitle("MauiApp7test - Index");
#endif


            AppDomain curDomain = AppDomain.CurrentDomain;
            Console.WriteLine("------------- Inspection Context --------------");
            var a = Assembly.Load("BlazorShared");

        }
    }
  
}


