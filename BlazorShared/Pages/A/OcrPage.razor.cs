using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Diagnostics;

namespace BlazorShared.Pages;

public partial class OcrPage
{
    [Inject] IConfiguration? config { get; set; }
    string? AzureCvKey;
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            AzureCvKey= config!["AzureCvKey"].Substring(0,6);
            StateHasChanged();
        }
    }

}
