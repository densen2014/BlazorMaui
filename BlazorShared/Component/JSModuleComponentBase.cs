// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using System.Reflection;

namespace BlazorShared.Shared;

/// <summary>
/// 
/// </summary>
public class JSModuleComponentBase : BootstrapModule2ComponentBase
{
    private static string? _tick;
    private static string? GetVersion()
    {
        _tick ??= DateTime.Now.ToString("HHmmss");
        return _tick;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnLoadJSModule()
    {
        var type = this.GetType();
        var attr = type.GetCustomAttribute<JSModuleAutoLoaderAttribute>();

        if (attr != null)
        {
            string? typeName = null;
            ModulePath = attr.Path ?? GetTypeName().ToLowerInvariant();
            ModulePath = $"./_content/BlazorShared/modules/{ModulePath}.js?v={GetVersion()}";
            ModuleName = attr.ModuleName ?? GetTypeName();
            JSObjectReference = attr.JSObjectReference;
            Relative = false;

            string GetTypeName()
            {
                typeName ??= type.GetTypeModuleName();
                return typeName;
            }
        }
    }
}
