using AIStudio.BlazorUI.Converter;
using AIStudio.BlazorUI.Models.CommonForms;
using AIStudio.BlazorUI.Models.Settings;
using AIStudio.Client.Business;
using AIStudio.Entity;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.Enum;
using AIStudio.Util;
using AIStudio.Util.Common;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Data;
using System.Dynamic;

namespace AIStudio.BlazorUI.Core
{
    public class CommonBaseList : PageBase
    {
        [Inject]
        protected IDataProvider DataProvider { get; set; }
        [Inject]
        protected AntDesign.ModalService ModalService { get; set; }
        [Inject]
        protected IUserData UserData { get; set; }
        [Inject]
        protected IOptions<LayoutSettings> LayoutSettings { get; set; }

        [Parameter]
        public string Para { get; set; }

        protected string Area { get; set; }
        protected string Name { get; set; }

        protected string ConfigUrl { get; set; } = "/Base_Manage/Base_CommonFormConfig/GetDataList";
        protected string GetDataList { get; set; } = "GetDataList";

        protected List<Dictionary<string, object>> Data { get; set; }

        private Dictionary<string, object> _selectedItem;
        protected Dictionary<string, object> SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; BaseControlItem.ObjectToList(_selectedItem, EditFormItems); }
        }

        protected IEnumerable<Dictionary<string, object>> SelectedItems { get; set; }

        protected bool NoneSelectedItems { get { return !(SelectedItems?.Count() > 0); } }

        protected AntDesign.Table<Dictionary<string, object>>? _table;

        protected List<QueryConditionItem> QueryConditionItems { get; set; }
        protected List<DataGridColumnCustom> DataGridColumns { get; set; }
        protected List<EditFormItem> EditFormItems { get; set; }

        protected int ScrollX { get; set; } = 1000;
        protected int ScrollY { get; set; } = 800;

        protected string IdName { get; set; } = "Id";
        protected bool IsNew { get { return SelectedItem == null || !SelectedItem.ContainsKey(IdName) || string.IsNullOrEmpty(SelectedItem[IdName]?.ToString()); } }

        protected override async Task OnParametersSetAsync()
        {
            var items = Para.Split(".");
            Area = items[0];
            Name = items[1];

            using (var waitfor = WaitFor.GetWaitFor(this))
            {
                await GetConfig();
                await GetData();

                await base.OnParametersSetAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


        protected virtual void OnRowClick(RowData<Dictionary<string, object>> dataRow)
        {
            if (dataRow == null || dataRow.Data == null)
                return;

            SelectedItem = dataRow.Data;
        }

        protected virtual async Task GetConfig()
        {
            try
            {
                var data = new
                {
                    PageIndex = 0,
                    PageRows = 500,
                    SearchKeyValues = new Dictionary<string, object>()
                    {
                        {"Table", Name }
                    }
                };

                var result = await DataProvider.PostData<List<Base_CommonFormConfigDTO>>(ConfigUrl, data.ToJson());
                if (!result.Success)
                {
                    throw new MsgException(result.Msg);
                }

                QueryConditionItems = new List<QueryConditionItem>(result.Data.Where(p => p.Type == 0).OrderBy(p => p.DisplayIndex).Select((p, index) => GetQueryConditionItem(p)));
                QueryConditionItems.Add(new QueryConditionItem() { Header = "新增", ControlType = ControlType.Add, Visibility = 0 });
                QueryConditionItems.Add(new QueryConditionItem() { Header = "复制", ControlType = ControlType.Copy, Visibility = 0});
                QueryConditionItems.Add(new QueryConditionItem() { Header = "删除", ControlType = ControlType.Delete, Visibility = 0 });
                QueryConditionItems.Add(new QueryConditionItem() { Header = "查询", ControlType = ControlType.Query, Visibility = 0 });

                DataGridColumns = new List<DataGridColumnCustom>(result.Data.Where(p => p.Type == 1).OrderBy(p => p.DisplayIndex).Select((p, index) => GetDataGridColumnCustom(p)));
                ScrollX = DataGridColumns.Count * 200;

                EditFormItems = new List<EditFormItem>(result.Data.Where(p => p.Type == 1).OrderBy(p => p.DisplayIndex).Select((p, index) => GetEditFormItem(p)));
                EditFormItems.Add(new EditFormItem() { Header = "提交", ControlType = ControlType.Submit, Visibility = 0 });
            }
            catch (Exception ex)
            {
                await Error?.ProcessError(ex);
            }
        }

        protected virtual string GetDataJson()
        {
            var data = new
            {
                PageIndex = Pagination.PageIndex,
                PageRows = Pagination.PageRows,
                SortField = Pagination.SortField,
                SortType = Pagination.SortType,
                SearchKeyValues = QueryConditionItem.ListToDictionary(QueryConditionItems),
            };

            return data.ToJson();
        }

        protected override async Task GetData()
        {
            using (var waitfor = WaitFor.GetWaitFor(this))
            {
                try
                {
                    var result = await DataProvider.PostData<List<Dictionary<string, object>>>($"/{Area}/{Name}/{GetDataList}", GetDataJson());
                    if (!result.Success)
                    {
                        throw new MsgException(result.Msg);
                    }
                    else
                    {
                        Pagination.Total = result.Total;
                        Data = result.Data;

                        if (SelectedItem != null && SelectedItem.ContainsKey(IdName))
                        {
                            SelectedItem = Data.FirstOrDefault(p => p[IdName] == SelectedItem[IdName]);
                        }
                        else if (Data?.Any() == true)
                        {
                            SelectedItem = Data.FirstOrDefault();
                        }
                        else
                        {
                            SelectedItem = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Error?.ProcessError(ex);
                }
            }
        }

        public void Submit()
        {
            Func<ModalClosingEventArgs, Task> onOkClick = async (e) =>
            {
                var obj = SelectedItem;
                if (SelectedItem == null)
                {
                    obj = new Dictionary<string, object>();
                }
                else
                {
                    obj = SelectedItem.DeepClone();
                }

                BaseControlItem.ListToObject(obj, EditFormItems);

                if (obj.ContainsKey("Error") && !string.IsNullOrEmpty(obj["Error"]?.ToString()))
                {
                    await Error?.ProcessError(new MsgException(obj["Error"]?.ToString()));
                    return;
                }

                await SaveData(obj);
            };

            RenderFragment icon = (builder) =>
            {
                var index = 0;
                IEnumerable<KeyValuePair<string, object>> paramenter = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("Type","check-circle")
                };

                builder.OpenComponent(index++, typeof(Icon));
                builder.AddMultipleAttributes(index++, paramenter);
                builder.CloseComponent();

            };
            ModalService.Confirm(new ConfirmOptions()
            {
                Title = SelectedItem == null ? "新增数据" : "修改数据",
                Icon = icon,
                Content = SelectedItem == null ? "确定要提交新增数据？" : "确定要提交修改数据？",
                Centered = true,
                OnOk = onOkClick
            });
        }

        protected virtual async Task SaveData(Dictionary<string, object> para)
        {
            using (var waitfor = WaitFor.GetWaitFor(this))
            {
                try
                {
                    var result = await DataProvider.PostData<AjaxResult>($"/{Area}/{Name}/SaveData", para.ToJson());
                    if (!result.Success)
                    {
                        throw new MsgException(result.Msg);
                    }
                    await GetData();
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    await Error?.ProcessError(ex);
                }
            }
        }

        protected virtual async Task Delete()
        {
            if (SelectedItems?.Count() > 0 && SelectedItems.First().ContainsKey(IdName))
            {
                await Delete(SelectedItems.Select(p => p[IdName]?.ToString()).ToList());
            }
        }

        protected virtual async Task Delete(List<string> ids)
        {
            using (var waitfor = WaitFor.GetWaitFor(this))
            {
                try
                {

                    var result = await DataProvider.PostData<AjaxResult>($"/{Area}/{Name}/DeleteData", ids.ToJson());
                    if (!result.Success)
                    {
                        throw new MsgException(result.Msg);
                    }
                    await GetData();
                }
                catch (Exception ex)
                {
                    await Error?.ProcessError(ex);
                }
            }
        }

        public async Task OnBtnClick(ControlType type)
        {
            switch (type)
            {
                case ControlType.Query: Refresh(); break;
                case ControlType.Delete: await Delete(); break;
                case ControlType.Submit: Submit(); break;
                case ControlType.Add:
                    {
                        SelectedItem = null;
                    }
                    break;
                case ControlType.Copy:
                    {
                        var idItem = EditFormItems.FirstOrDefault(p => p.PropertyName == IdName);
                        if (idItem != null)
                            idItem.Value = null;
                        _selectedItem = null;//不要触发EditFormItems赋值事件
                        break;
                    }

            }
        }

        private DataGridColumnCustom GetDataGridColumnCustom(Base_CommonFormConfigDTO config)
        {
            var itemsource = config.ItemSource ?? config.PropertyName;

            DataGridColumnCustom item = new DataGridColumnCustom();
            item.Header = config.Header;
            item.PropertyName = config.PropertyName;
            item.StringFormat = config.StringFormat;
            item.Visibility = config.Visibility;
            item.SortMemberPath = config.SortMemberPath;
            item.Converter = config.Converter;
            item.ConverterParameter = config.ConverterParameter ?? config.ItemSource ?? config.PropertyName;
            item.HorizontalAlignment = config.HorizontalAlignment;
            item.MaxWidth = config.MaxWidth;
            item.MinWidth = config.MinWidth;
            item.Width = config.Width;
            item.CanUserReorder = config.CanUserReorder;
            item.CanUserResize = config.CanUserResize;
            item.CanUserSort = config.CanUserSort;
            item.CellStyle = config.CellStyle;
            item.HeaderStyle = config.HeaderStyle;
            item.BackgroundExpression = config.BackgroundExpression;
            item.ForegroundExpression = config.ForegroundExpression;
            if (string.IsNullOrEmpty(config.Converter))
            {
                if (UserData.ItemSource.ContainsKey(itemsource))
                {
                    item.Converter = typeof(ObjectToStringConverter).Name;
                    item.ConverterParameter = itemsource;
                }
            }
            return item;
        }

        private EditFormItem GetEditFormItem(Base_CommonFormConfigDTO config)
        {
            EditFormItem item = new EditFormItem();
            GetBaseControlItem(item, config);

            return item;
        }

        private QueryConditionItem GetQueryConditionItem(Base_CommonFormConfigDTO config)
        {
            QueryConditionItem item = new QueryConditionItem();
            GetBaseControlItem(item, config);

            return item;
        }

        private void GetBaseControlItem(BaseControlItem item, Base_CommonFormConfigDTO config)
        {
            var itemsource = config.ItemSource ?? config.PropertyName;

            item.Header = config.Header;
            item.PropertyName = config.PropertyName;
            item.Value = config.Value;
            item.Visibility = config.Visibility;
            item.ControlType = config.ControlType;
            item.IsRequired = config.IsRequired;
            item.StringFormat = config.StringFormat;
            item.IsReadOnly = config.IsReadOnly;
            item.Regex = config.Regex;
            item.ErrorMessage = config.ErrorMessage;

            if (!string.IsNullOrEmpty(itemsource))
            {
                if (UserData.ItemSource.ContainsKey(itemsource))
                {
                    //树形控件使用树形数据集
                    if (config.ControlType == ControlType.TreeSelect || config.ControlType == ControlType.MultiTreeSelect)
                    {
                        item.ItemSource = UserData.ItemSource[$"{itemsource}Tree"];
                    }
                    else
                    {
                        item.ItemSource = UserData.ItemSource[itemsource];
                    }
                }
            }

            if (!string.IsNullOrEmpty(config.PropertyType))
            {
                if (config.PropertyType.ToLower() == "int" || config.PropertyType.ToLower() == "int?")
                {
                    if (string.IsNullOrEmpty(config.StringFormat))
                    {
                        item.StringFormat = "n0";
                    }
                    if (item.ControlType == ControlType.None)
                    {
                        item.ControlType = ControlType.IntegerUpDown;
                    }
                }
                else if (config.PropertyType.ToLower() == "long" || config.PropertyType.ToLower() == "long?")
                {
                    if (string.IsNullOrEmpty(item.StringFormat))
                    {
                        item.StringFormat = "n0";
                    }
                    if (item.ControlType == ControlType.None)
                    {
                        item.ControlType = ControlType.LongUpDown;
                    }
                }
                else if (config.PropertyType.ToLower() == "double" || config.PropertyType.ToLower() == "double?")
                {
                    if (string.IsNullOrEmpty(item.StringFormat))
                    {
                        item.StringFormat = "f3";
                    }
                    if (item.ControlType == ControlType.None)
                    {
                        item.ControlType = ControlType.DoubleUpDown;
                    }
                }
                else if (config.PropertyType.ToLower() == "decimal" || config.PropertyType.ToLower() == "decimal?")
                {
                    if (string.IsNullOrEmpty(item.StringFormat))
                    {
                        item.StringFormat = "f3";
                    }
                    if (item.ControlType == ControlType.None)
                    {
                        item.ControlType = ControlType.DecimalUpDown;
                    }
                }
                else if (config.PropertyType.ToLower() == "datetime" || config.PropertyType.ToLower() == "datetime?")
                {
                    if (string.IsNullOrEmpty(item.StringFormat))
                    {
                        item.StringFormat = "yyyy-MM-dd HH:mm:ss";
                    }
                    if (item.ControlType == ControlType.None)
                    {
                        item.ControlType = ControlType.DateTimeUpDown;
                    }
                }
            }

            if (item.ControlType == ControlType.None)
            {
                item.ControlType = ControlType.TextBox;
            }
        }

    }
}
