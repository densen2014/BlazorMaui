// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

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
        }
    }
  
}


