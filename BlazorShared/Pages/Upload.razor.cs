// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using BlazorShared.Services;
using AmeApi;
using AmeBlazor.Components;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorShared.Pages
{
    public partial class Upload
    {


        private static readonly Random random = new();

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }


        private List<UploadFile> PreviewFileList { get; } = new(new[] { new UploadFile { PrevUrl = "_content/BootstrapBlazor.Shared/images/Argo.png" } });


        private List<UploadFile> DefaultFormatFileList { get; } = new List<UploadFile>()
        {
            new UploadFile { FileName = "Test.xls" },
            new UploadFile { FileName = "Test.doc" },
            new UploadFile { FileName = "Test.ppt" },
            new UploadFile { FileName = "Test.mp3" },
            new UploadFile { FileName = "Test.mp4" },
            new UploadFile { FileName = "Test.pdf" },
            new UploadFile { FileName = "Test.cs" },
            new UploadFile { FileName = "Test.zip" },
            new UploadFile { FileName = "Test.txt" },
            new UploadFile { FileName = "Test.dat" }
        };

        private Task OnFileChange(UploadFile file)
        {
            // 未真正保存文件
            // file.SaveToFile()
            System.Console.WriteLine($"{file.File!.Name} Success");
            return Task.FromResult("");
        }

        private async Task OnClickToUpload(UploadFile file)
        {
            // 示例代码，模拟 80% 几率保存成功
            var error = random.Next(1, 100) > 80;
            if (error)
            {
                file.Code = 1;
                file.Error = "Error";
            }
            else
            {
                await SaveToFile(file);
            }
        }

        private CancellationTokenSource? UploadFolderToken { get; set; }
        private async Task OnUploadFolder(UploadFile file)
        {
            // 上传文件夹时会多次回调此方法
            await SaveToFile(file);
        }

        private CancellationTokenSource? ReadAvatarToken { get; set; }
        private async Task OnAvatarUpload(UploadFile file)
        {
            // 示例代码，使用 base64 格式
            if (file != null && file.File != null)
            {
                var format = file.File.ContentType;
                if (CheckValidAvatarFormat(format))
                {
                    ReadAvatarToken ??= new CancellationTokenSource();
                    if (ReadAvatarToken.IsCancellationRequested)
                    {
                        ReadAvatarToken.Dispose();
                        ReadAvatarToken = new CancellationTokenSource();
                    }

                    await file.RequestBase64ImageFileAsync(format, 640, 480, MaxFileLength, ReadAvatarToken.Token);
                }
                else
                {
                    file.Code = 1;
                    file.Error = "FormatError";
                }

                if (file.Code != 0)
                {
                    await ToastService.Error("AvatarMsg", $"{file.Error} {format}");
                }
            }
        }

        private CancellationTokenSource? ReadToken { get; set; }

        private static long MaxFileLength => 200 * 1024 * 1024;

        private async Task OnCardUpload(UploadFile file)
        {
            if (file != null && file.File != null)
            {
                // 服务器端验证当文件大于 2MB 时提示文件太大信息
                if (file.Size > MaxFileLength)
                {
                    await ToastService.Information("FileMsg", "FileError");
                    file.Code = 1;
                    file.Error = "FileError";
                }
                else
                {
                    await SaveToFile(file);
                }
            }
        }

        string WebRootPath = "d:\\www";

        private async Task<bool> SaveToFile(UploadFile file)
        {
            // Server Side 使用
            // Web Assembly 模式下必须使用 webapi 方式去保存文件到服务器或者数据库中
            // 生成写入文件名称
            var ret = false;
            if (!string.IsNullOrEmpty(WebRootPath))
            {
                var uploaderFolder = Path.Combine(WebRootPath, $"images{Path.DirectorySeparatorChar}uploader");
                file.FileName = $"{Path.GetFileNameWithoutExtension(file.OriginFileName)}-{DateTimeOffset.Now:yyyyMMddHHmmss}{Path.GetExtension(file.OriginFileName)}";
                var fileName = Path.Combine(uploaderFolder, file.FileName);

                ReadToken ??= new CancellationTokenSource();
                ret = await file.SaveToFile(fileName, MaxFileLength, ReadToken.Token);

                if (ret)
                {
                    // 保存成功
                    file.PrevUrl = $"images/uploader/{file.FileName}";
                }
                else
                {
                    var errorMessage = $"{"SaveFileError"} {file.OriginFileName}";
                    file.Code = 1;
                    file.Error = errorMessage;
                    await ToastService.Error("UploadFile", errorMessage);
                }
            }
            else
            {
                file.Code = 1;
                file.Error = "WasmError";
                await ToastService.Information("SaveFile", "SaveFileMsg");
            }
            return ret;
        }

        private static bool CheckValidAvatarFormat(string format)
        {
            return "jpg;png;bmp;gif;jpeg".Split(';').Any(f => format.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        private Task<bool> OnFileDelete(UploadFile item)
        {
            System.Console.WriteLine($"{item.OriginFileName} {"RemoveMsg"}");
            return Task.FromResult(true);
        }

        private Person Foo { get; set; } = new Person();

        private static Task OnSubmit(EditContext context)
        {
            // 示例代码请根据业务情况自行更改
            // var fileName = Foo.Picture?.Name;
            return Task.CompletedTask;
        }


        private Task OnAvatarValidSubmit(EditContext context)
        {
            System.Console.WriteLine(Foo.Picture?.Name ?? "");
            return Task.CompletedTask;
        }

        private class Person
        {
            [Required]
            [StringLength(20, MinimumLength = 2)]
            public string Name { get; set; } = "Blazor";

            [Required]
            [FileValidation(Extensions = new string[] { ".png", ".jpg", ".jpeg" }, FileSize = 50 * 1024)]
            public IBrowserFile? Picture { get; set; }
        }

        [Inject]
        [NotNull]
        private FullScreenService? FullScreenService { get; set; }

        private async Task ToggleFullScreen()
        {
            await FullScreenService.Toggle();
        }

        private string Value3 { get; set; } = "#DDDDDD";

        [Inject]
        [NotNull]
        private DownloadService? downloadService { get; set; }
        private async Task DownloadFileAsync()
        {
            var content = await GenerateFileAsync();
            await downloadService.DownloadAsync("测试文件", content);

            static async Task<byte[]> GenerateFileAsync()
            {
                using var ms = new MemoryStream();
                using var writer = new StreamWriter(ms);
                await writer.WriteLineAsync("自行生成并写入的文本，这里可以换成图片或其他内容");
                await writer.FlushAsync();
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        private Task OnTimeout()
        {
            System.Console.WriteLine("计时器时间到");
            return Task.CompletedTask;
        }

        private Task OnCancel()
        {
            System.Console.WriteLine("计时器取消");
            return Task.CompletedTask;
        }
    }
}


