using BootstrapBlazor.Components;
using BlazorShared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using LibraryShared;

namespace BlazorShared.Pages;

/// <summary>
/// 
/// </summary>
public partial class TableDemo : ComponentBase
{
    [Inject]
    [NotNull]
#nullable enable
    private IStringLocalizer<Foo>? Localizer { get; set; }
#nullable disable 

    private readonly ConcurrentDictionary<Foo, IEnumerable<SelectedItem>> _cache = new();

    private IEnumerable<SelectedItem> GetHobbys(Foo item) => _cache.GetOrAdd(item, f => Foo.GenerateHobbys(Localizer));

    /// <summary>
    /// 
    /// </summary>
    private static IEnumerable<int> PageItemsSource => new int[] { 20, 40 };
    protected override void OnInitialized()
    {
        DataService dataService = new DataService();
        var fsql = dataService.Initfsql();
        if (fsql.Select<Foo>().Count() < 4)
        {
            var itemList = Foo.GenerateFoo(Localizer).Cast<Foo>().ToList();
            fsql.Insert<Foo>().AppendData(itemList).ExecuteAffrows();
        } 

    }

}
