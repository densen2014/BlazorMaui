using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Diagnostics;

namespace DemoShared.Pages;

public sealed partial class SignaturePadPageResponsive
{
    string? SaveResult;
    [Inject] IHostingEnvironment? Environment { get; set; }
    public async Task<string?> SaveImgBaseSixFour(string? img)
    {
        try
        {
            //var SignatureDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "signature");
            var SignatureDir = Path.Combine(Environment!.ContentRootPath, "wwwroot", "signature");
            if (!Directory.Exists(SignatureDir)) Directory.CreateDirectory(SignatureDir);

            if (img == null)
            {
                return "数据为空";
            }
            //判断是不是base64文件类型
            int index = img.IndexOf("base64,");
            if (index != -1)
            {
                index += 7;
                //将数据转换为二进制字节数组
                var imgbit = Convert.FromBase64String(img.Substring(index));
                //生成文件名
                string imgname = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                //保存图片
                await File.WriteAllBytesAsync(Path.Combine(SignatureDir, imgname), imgbit);
                return $"signature/{imgname}";
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }
        return null;
    }

}
