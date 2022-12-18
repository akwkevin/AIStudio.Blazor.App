using AIStudio.BlazorUI.Pages.Base_Manage;
using AIStudio.Client.Business;
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

namespace AIStudio.BlazorUI.Core
{
    public class BaseListWithEdit<TData, EditForm> : BaseList<TData> where TData : class, IKeyBaseEntity where EditForm : FeedbackComponent<object>
    {
        protected double EditWitdh { get; set; } = 520d;
        protected bool DefaultFooter { get; set; } = true;
        protected string NewTitle { get; set; } = "新建表单";
        protected string EditTitle  { get; set; } = "编辑表单";

        protected override async void Edit(TData? para = null)
        {
            var modalConfig = new ModalOptions();
            modalConfig.Title = para == null ? NewTitle : EditTitle;
            //modalConfig.Style = "top:20px;";
            // In order for Dispose in ConfirmTemplateDemo to take effect every time it is closed
            //modalConfig.BodyStyle += "overflow-y: auto;";
            modalConfig.DestroyOnClose = true;
            modalConfig.Centered = true;
            modalConfig.Width = EditWitdh;
            if (DefaultFooter == false)
            {
                modalConfig.Footer = null;
            }
            var modalRef = await ModalService.CreateModalAsync<EditForm, object>(modalConfig, para?.Id);

            modalRef.OnOk = async () =>
            {
                await GetData();
                StateHasChanged();
            };
        }
    }
}
