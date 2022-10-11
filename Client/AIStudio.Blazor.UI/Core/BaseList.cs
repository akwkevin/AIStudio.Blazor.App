using AIStudio.Client.Business;
using AIStudio.Util;
using AIStudio.Util.Common;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        protected string Condition { get; set; }
        protected string KeyWord { get; set; }
        protected List<TData> Data { get; set; }
        protected TData SelectedItem { get; set; }
        protected IEnumerable<TData> SelectedItems { get; set; }
        protected bool NoneSelectedItems { get { return !(SelectedItems?.Count() > 0); } }
        protected Table<TData> _table;

        protected virtual string GetDataJson()
        {
            var searchKeyValues = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Condition) && !string.IsNullOrEmpty(KeyWord))
            {
                searchKeyValues.Add(Condition, KeyWord);
            }

            var data = new
            {
                PageIndex = Pagination.PageIndex,
                PageRows = Pagination.PageRows,
                SortField = Pagination.SortField,
                SortType = Pagination.SortType,
                SearchKeyValues = searchKeyValues,
            };

            return data.ToJson();
        }

        protected override async Task GetData()
        {
            try
            {
                ShowWait();

                var result = await DataProvider.GetData<List<TData>>($"/{Area}/{typeof(TData).Name.Replace("DTO", "").Replace("Tree","")}/{GetDataList}", GetDataJson());
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
