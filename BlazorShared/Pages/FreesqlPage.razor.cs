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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LibraryShared;
using System.Diagnostics.CodeAnalysis;

namespace BlazorShared.Pages
{
    public partial class FreesqlPage
    {
        List<TaskItem> ItemList = new List<TaskItem>();
        [NotNull]
        DataService? dataService { get; set; }
        string CounterLabel = "Current count: 0";
        TaskItem? ItemSelected;
        int count = 0;
        protected override void OnInitialized()
        {
            dataService = new DataService();
            ItemList = dataService.Init(initdemodatas: true);

        }

        private void OnCounterClicked()
        {
            count = ItemList.Count;
            CounterLabel = $"Current count: {count}";

        }
        private void OnSelectOneClicked()
        {
            ItemSelected = null;// ItemList[3];
            CounterLabel = $"Select item: {ItemSelected}";

        }

        private void OnAddClicked()
        {
            var item = dataService.Add($"New {count + 1}");
            ItemList.Add(item);

            OnCounterClicked();
        }
        private void OnRefreshClicked()
        {
            ItemList.Clear();
            foreach (var item in dataService.Init())
            {
                this.ItemList.Add(item);
            }
            OnCounterClicked();
        }

        private void OnModifyClicked()
        {
            ItemList[0] = dataService.Modify() ?? ItemList[0];
            OnCounterClicked();
        }

        private void OnDeleteClicked()
        {
            if (dataService.Delete()) ItemList.RemoveAt(ItemList.Count - 1);
            OnCounterClicked();
        }
    }
}


