using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MrHardware.Shared;

[JSModuleAutoLoader("/modules/menu.js", ModuleName = "Menu", Relative = false)]
public partial class NavMenu
{
    private IEnumerable<MenuItem> Menus { get; set; } = new List<MenuItem>
    {
            new MenuItem() { Text = "首页", Url = "/"  , Match = NavLinkMatch.All},
            new MenuItem() { Text = "平台功能", Url = "/PlatformFeatures" },
            new MenuItem() { Text = "环境", Url = "/EnvironmentInfo" },
            new MenuItem() { Text = "工具" ,Items= new List<MenuItem>
                {
                    new MenuItem() { Text = "文件预览 FileViewer", Url = "/fileViewers" },
                    new MenuItem() { Text = "PDF阅读器 PDF Reader", Url = "/pdfReaders" },
               }
            },
    };
}
