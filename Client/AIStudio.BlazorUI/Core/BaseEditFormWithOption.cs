using AIStudio.BlazorUI.Shared;
using AIStudio.Client.Business;
using AIStudio.Util;
using AIStudio.Util.Common;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorUI.Core
{
    public class BaseEditFormWithOption<TData,Option> : FeedbackComponent<Option>
    {
        [Inject]
        protected IDataProvider DataProvider { get; set; }
        [Inject]
        protected IUserData UserData { get; set; }
        [Inject]
        protected MessageService MessageService { get; set; }

        [CascadingParameter]
        public Error? Error { get; set; }

        protected bool Disabled { get; set; }
        protected bool Loading { get; set; }
        protected string Area { get; set; }

        protected TData Data { get; set; }

        protected Form<TData> _form;

        public BaseEditFormWithOption()
        {
            try
            {
                Data = System.Activator.CreateInstance<TData>();
            }
            catch(Exception ex) { }
        }

        protected virtual async Task GetDataAsync(Option option)
        {
            try
            {
                ShowWait();

                if (option == null)
                {
                    Data = System.Activator.CreateInstance<TData>();
                }
                else
                {
                    if (option is string id)
                    {
                        var result = await DataProvider.PostData<TData>($"/{Area}/{typeof(TData).Name.Replace("DTO", "")}/GetTheData", (new { id = id }).ToJson());
                        if (!result.Success)
                        {
                            throw new MsgException(result.Msg);
                        }
                        Data = result.Data;
                    }
                    else if (option is TData data)
                    {     
                        Data = data;
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

        protected virtual async Task SaveData(TData para)
        {
            try
            {
                ShowWait();
                var result = await DataProvider.PostData<AjaxResult>($"/{Area}/{typeof(TData).Name.Replace("DTO", "")}/SaveData", para.ToJson());
                if (!result.Success)
                {
                    throw new MsgException(result.Msg);
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

        protected void ShowWait()
        {
            Loading = true;
        }

        protected void HideWait()
        {
            Loading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var id = this.Options;
            if (id != null)
            {
                await GetDataAsync(id);
            }
            await base.OnInitializedAsync();
        }


        public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
        {
            try
            {
                if (FeedbackRef is ConfirmRef confirmRef)
                {
                    confirmRef.Config.OkButtonProps.Loading = true;
                    await confirmRef.UpdateConfigAsync();
                }
                else if (FeedbackRef is ModalRef modalRef)
                {
                    modalRef.Config.ConfirmLoading = true;
                    await modalRef.UpdateConfigAsync();
                }

                if (_form.Validate())
                {
                    await SaveData(Data);
                }
                else
                {
                    args.Cancel = true;
                }

                await base.OnFeedbackOkAsync(args);
            }
            finally
            {
                if (FeedbackRef is ConfirmRef confirmRef)
                {
                    confirmRef.Config.OkButtonProps.Loading = false;
                    await confirmRef.UpdateConfigAsync();
                }
                else if (FeedbackRef is ModalRef modalRef)
                {
                    modalRef.Config.ConfirmLoading = false;
                    await modalRef.UpdateConfigAsync();
                }
            }
        }

        /// <summary>
        /// If you want <b>Dispose</b> to take effect every time it is closed in Modal, which created by ModalService,
        /// set <b>ModalOptions.DestroyOnClose = true</b>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public AntDesign.RadioOption<enumT>[] GetRadioOptions<enumT>() where enumT : Enum
        {
            List<AntDesign.RadioOption<enumT>> options = new List<AntDesign.RadioOption<enumT>>();

            foreach (enumT o in Enum.GetValues(typeof(enumT)))
            {
                options.Add(new AntDesign.RadioOption<enumT>() { Value = o, Label = o.GetDescription() });
            }
            return options.ToArray();
        }
    }
}
