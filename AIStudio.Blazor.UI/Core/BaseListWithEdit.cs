using AIStudio.Blazor.UI.Pages.Base_Manage;
using AIStudio.Business;
using AIStudio.Entity;
using AIStudio.Util.Common;
using AIStudio.Util;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AIStudio.Blazor.UI.Core
{
    public class BaseListWithEdit<TData, EditForm> : BaseList<TData> where TData : IIdObject where EditForm : FeedbackComponent<string>
    {
        protected override async void Edit(TData para)
        {
            var modalConfig = new ModalOptions();
            modalConfig.Title = "编辑表单";
            //modalConfig.Style = "top:20px;";
            // In order for Dispose in ConfirmTemplateDemo to take effect every time it is closed
            //modalConfig.BodyStyle += "overflow-y: auto;";
            modalConfig.DestroyOnClose = true;
            modalConfig.Centered = true;

            var modalRef = await ModalService.CreateModalAsync<EditForm, string>(modalConfig, para?.Id);

            modalRef.OnOk = async () =>
            {
                await GetData();
                StateHasChanged();
            };
        }
    }
}
