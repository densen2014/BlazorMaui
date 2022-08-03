// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using System;

namespace BlazorShared.Data;

/// <summary>
/// 页面
/// </summary>
[AutoGenerateClass(Searchable = true, Filterable = true, Sortable = true)]
public class WebPages
{
    public WebPages() { }
    public WebPages(string PageName,  string? Url = null, string? Icon = null, string? Code = "0", List<WebPages>? Childs = null)
    {
        this.PageName = PageName;
        this.Url = Url ?? $"/{PageName}";
        this.Icon = Icon;
        this.Code = Code;
        this.Childs = Childs;
    }

    /// <summary>
    ///代码
    /// </summary>
    [DisplayName("代码")]
    [Column(IsPrimary = true)]
    [AutoGenerateColumn(DefaultSort = true, DefaultSortOrder = SortOrder.Asc)]
    public string? Code { get; set; }

    /// <summary>
    ///父级代码
    /// </summary>
    [DisplayName("父级代码")]
    [Column]
    public string? ParentCode { get; set; }

    [Navigate(nameof(ParentCode))]
    [AutoGenerateColumn(Ignore = true)]
    public WebPages? Parent { get; set; }

    [Navigate(nameof(ParentCode))]
    [AutoGenerateColumn(Ignore = true)]
    public List<WebPages>? Childs { get; set; }

    /// <summary>
    ///页面名称
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("页面名称")]
    public string? PageName { get; set; }

    /// <summary>
    ///Icon
    /// </summary>
    [DisplayName("Icon")]
    [AutoGenerateColumn(Visible = false)]
    public string? Icon { get; set; }

    /// <summary>
    ///Url
    /// </summary>
    [Required(ErrorMessage = "{0}不能为空")]
    [DisplayName("Url")]
    [AutoGenerateColumn(Visible = false)]
    public string? Url { get; set; } 

    /// <summary>
    /// 隐藏
    /// </summary>
    [DisplayName ("隐藏")]
    public bool Hide { get; set; } 

}


//public static class Utility
//{
//    /// <summary>
//    /// 菜单树状数据层次化方法
//    /// </summary>
//    /// <param name="items">数据集合</param>
//    /// <param name="parentId">父级节点</param>
//    public static IEnumerable<MenuItem> CascadingMenu(this IEnumerable<MenuItem> items, string? parentId = null) => items.Where(i => i.ParentId == parentId).Select(i =>
//    {
//        i.Items = CascadingMenu(items, i.Id).ToList();
//        return i;
//    });
//}
