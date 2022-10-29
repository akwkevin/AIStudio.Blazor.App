using AIStudio.BlazorUI.Shared;
using AIStudio.Client.Business;
using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Core
{
    public class PageBase : LoadingBase, IReuseTabsPage
    {

        [Inject]
        protected IOperator Operator { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public Error? Error { get; set; }


        protected string IndexUrl { get; set; }

        protected AIStudio.Util.Common.Pagination Pagination { get; set; } = new AIStudio.Util.Common.Pagination() { PageRows = 100 };
        protected virtual Func<PaginationTotalContext, string> ShowTotal { get; set; } = ctx => $"总数：{ctx.Total}   当前：{ctx.Range.from}-{ctx.Range.to}";

        protected virtual async Task PageIndexChanged(PaginationEventArgs paginationEvent)
        {
            if (Pagination.PageIndex != paginationEvent.Page)
            {
                Pagination.PageIndex = paginationEvent.Page;
                await GetData();
            }
        }

        protected virtual async Task PageSizeChanged(PaginationEventArgs paginationEvent)
        {
            if (Pagination.PageRows != paginationEvent.PageSize)
            {
                Pagination.PageRows = paginationEvent.PageSize;
                await GetData();
            }
        }

        protected virtual async Task GetData()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task Refresh()
        {
            Pagination.PageIndex = 0;
            await GetData();
        }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(IndexUrl))
            {
                IndexUrl = NavigationManager.Uri;
                var uri = new Uri(IndexUrl);
                IndexUrl = uri.LocalPath;
            }
            await Task.CompletedTask;
        }

        public virtual RenderFragment GetPageTitle() => builder =>
        {
            var menu = Operator.Menus.FirstOrDefault(u => u.Url == IndexUrl);
            if (menu != null)
            {
                var index = 0;
                builder.OpenElement(index++, "div");
                if (!string.IsNullOrEmpty(menu.Icon))
                {
                    IEnumerable<KeyValuePair<string, object>> paramenter = new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("Type", menu.Icon)
                    };

                    builder.OpenComponent(index++, typeof(Icon));
                    builder.AddMultipleAttributes(index++, paramenter);
                    builder.CloseComponent();
                }
                builder.AddMarkupContent(index++, menu.Text);
                builder.CloseElement();
            }
        };
    }
}
