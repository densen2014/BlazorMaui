    BlazorMaui , 整合Blazor,BootstrapBlazor UI组件库,Maui的共享跨平台工程示例

用 c # 和 Razor 创建本机移动应用和桌面应用。可以快速开发共享代码库运行于 Windows (Winforms/WPF/UWP)、Android、iOS、macOS 的应用。

 [GitHub](https://github.com/densen2014/BlazorMaui") | [Gitee(mirror only)](https://gitee.com/alexchow/BlazorMaui")
 
> 在 Mac 上开发 .NET MAUI（全）<https://www.cnblogs.com/densen2014/p/16057571.html>

> 在 M1 Mac 上开发 .NET MAUI (iOS) <https://www.cnblogs.com/densen2014/p/16057563.html>

> 在 Windows 上开发 .NET MAUI <https://docs.microsoft.com/zh-cn/dotnet/maui/get-started/installation>
 
1.Blazor 简介  

Blazor 是一个使用 .NET 生成交互式客户端 Web UI 的框架：

使用 C# 代替 JavaScript 来创建信息丰富的交互式 UI。
共享使用 .NET 编写的服务器端和客户端应用逻辑。
将 UI 呈现为 HTML 和 CSS，以支持众多浏览器，其中包括移动浏览器。
与新式托管平台（如 Docker）集成。
使用 .NET 进行客户端 Web 开发可提供以下优势：

使用 C# 代替 JavaScript 来编写代码。
利用现有的 .NET 库生态系统。
在服务器和客户端之间共享应用逻辑。
受益于 .NET 的性能、可靠性和安全性。
在 Windows、Linux 和 macOS 上使用 Visual Studio 保持高效工作。
以一组稳定、功能丰富且易用的通用语言、框架和工具为基础来进行生成


2.Bootstrap 风格的 Blazor UI 组件库, 以下简称BB

基于 Bootstrap 样式库精心打造，并且额外增加了 100 多种常用的组件，为您快速开发项目带来非一般的感觉,致力于打造全网最好用的,最好玩的组件库。


3.什么是 .NET MAUI？  

.NET 多平台应用 UI ( .NET MAUI) 是一个跨平台框架，用于使用 c # 和 XAML 创建本机移动应用和桌面应用。
使用 .net MAUI，可以开发可在 Android、iOS、macOS 上运行的应用，Windows 以及从单个共享代码库运行的应用。



------------------------------------

1.Introduction to Blazor

Blazor is a framework for building interactive client-side web UI with .NET:

Create rich interactive UIs using C# instead of JavaScript.
Share server-side and client-side app logic written in .NET.
Render the UI as HTML and CSS for wide browser support, including mobile browsers.
Integrate with modern hosting platforms, such as Docker.
Using .NET for client-side web development offers the following advantages:

Write code in C# instead of JavaScript.
Leverage the existing .NET ecosystem of .NET libraries.
Share app logic across server and client.
Benefit from .NET's performance, reliability, and security.
Stay productive with Visual Studio on Windows, Linux, and macOS.
Build on a common set of languages, frameworks, and tools that are stable, feature-rich, and easy to use.

2.Bootstrap style Blazor UI component library,Abbreviated as BB

Based on the Bootstrap style library, it is carefully built, and 100 a variety of commonly used components have been added to bring you an extraordinary feeling for rapid development projects.The best looking,native Blazor components library on the market

3.What is .NET MAUI? 

.NET Multi-platform App UI (.NET MAUI) is a cross-platform framework for creating native mobile and desktop apps with C# and XAML.
Using .NET MAUI, you can develop apps that can run on Android, iOS, macOS, and Windows from a single shared code-base.


![UWP](https://user-images.githubusercontent.com/8428709/148663562-3ecca526-38a0-430b-b0d9-58875bcc7887.png)
![BlazorMaui_Android](https://user-images.githubusercontent.com/8428709/148663564-e850ed36-d6e1-4c51-b958-068fcfff2ad0.png)
![BlazorSSR](https://user-images.githubusercontent.com/8428709/148663565-9647cecf-60f5-4543-b8f8-87b55a1a593e.png)
![BlazorWinForms](https://user-images.githubusercontent.com/8428709/148663566-5e35fbdb-1669-4967-8803-2763c3c6d2cd.png)
![BlazorWpf](https://user-images.githubusercontent.com/8428709/148663568-95cfdec8-3778-4f74-aa84-db4f08bafe09.png)


------------------------------------

2-24

Maui blazor 最新preview版本发布ok了,只是还不能打非依赖包和裁剪. 
发布后 安装net6 desktop rumtime + webview2 x64就能运行.

net6 desktop rumtime

https://download.visualstudio.microsoft.com/download/pr/efa32b7a-6eec-4d97-9cdc-c7336a29a749/3df4296170397cf60884dae1be3d103b/windowsdesktop-runtime-6.0.2-win-x64.exe

webview2

https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/4182949e-08fa-48c6-9845-edf4ff44767b/MicrosoftEdgeWebView2RuntimeInstallerX64.exe

![image](https://user-images.githubusercontent.com/8428709/155608453-b9eca2a9-7862-4ff2-b78f-c57ce1c3dad4.png)

