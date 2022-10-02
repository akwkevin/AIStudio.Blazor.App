using AIStudio.Client.Business;
using AIStudio.Common.Result;
using AIStudio.Util;
using AIStudio.Util.Common;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Xml.Linq;

namespace AIStudio.Blazor.UI.Core
{
    /// <summary>
    /// ListData 为表格中的数据
    /// TData为双击后编辑的数据，
    /// </summary>
    /// <typeparam name="ListData"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class BaseList<TData> : PageBase where TData : IIdObject
    {

        [Inject] 
        protected IDataProvider DataProvider { get; set; }
        [Inject] 
        protected MessageService MessageService { get; set; }
        [Inject] 
        protected ModalService ModalService { get; set; }

        [Inject] 
        IJSRuntime JSRuntime { get; set; }
        protected string Area { get; set; }

        protected string GetDataList { get; set; } = "GetDataList";

        protected Type TDataType { get; set; } = typeof(TData);
        protected Type EditType { get; set; }

        protected string Condition { get; set; }


        protected string Keyword { get; set; }

        protected Dictionary<string, object> SearchKeyValues { get; set; } = new Dictionary<string, object>();

        protected List<TData> Data { get; set; }

        protected TData SelectedItem { get; set; }
        protected IEnumerable<TData> SelectedItems { get; set; }

        protected bool NoneSelectedItems { get { return !(SelectedItems?.Count() > 0); } }

        protected Table<TData> _table;

        protected override async Task GetData()
        {
            try
            {
                ShowWait();
                var data = new
                {
                    Pagination.PageIndex,
                    Pagination.PageRows,
                    Pagination.SortField,
                    Pagination.SortType,
                    Search = new
                    {
                        keyword = Keyword,
                        condition = Condition,
                    },
                    SearchKeyValues,
                };

                var result = await DataProvider.GetData<List<TData>>($"/{Area}/{typeof(TData).Name.Replace("DTO", "").Replace("Tree","")}/{GetDataList}", data.ToJson());
                if (!result.Success)
                {
                    throw new MsgException(result.Msg);
                }
                else
                {
                    Pagination.Total = result.Total;
                    Data = result.Data;
                    if (Data.Any())
                    {
                        SelectedItem = Data.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                await Error.ProcessError(ex);
            }
            finally
            {
                HideWait();
            }
        }

        protected virtual void Edit(TData para)
        {

        }

        protected virtual async Task Delete(string id)
        {
            await Delete(new List<string> { id });
        }

        protected virtual async Task Delete()
        {
            if (SelectedItems != null)
            {
                await Delete(SelectedItems.Select(p => p.Id).ToList());
            }
        }

        protected virtual async Task Delete(List<string> ids)
        {
            try
            {
                ShowWait();

                var result = await DataProvider.GetData<AjaxResult>($"/{Area}/{typeof(TData).Name.Replace("DTO", "")}/DeleteData", ids.ToJson());
                if (!result.Success)
                {
                    throw new MsgException(result.Msg);
                }
                await GetData();
            }
            catch (Exception ex)
            {
                await Error.ProcessError(ex);
            }
            finally
            {
                HideWait();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(IndexUrl))
            {
                IndexUrl = NavigationManager.Uri;
                var uri = new Uri(IndexUrl);
                IndexUrl = uri.LocalPath;
            }
            await base.OnInitializedAsync();
            await GetData();
        }
        protected override void OnParametersSet()
        {

        }
    }
}
