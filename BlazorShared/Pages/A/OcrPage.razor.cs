using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DemoShared.Pages;

public partial class OcrPage
{
    [Inject, NotNull] IConfiguration? config { get; set; }
    string? AzureCvKey;
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            AzureCvKey = config["AzureCvKey"];  
            AzureCvKey = AzureCvKey?.Substring(0, 6); 
            StateHasChanged();
        }
    }

}
