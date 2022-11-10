using AIStudio.Util;
using AIStudio.Util.Common;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Client.Business
{
    public class ApiDataProvider : IDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _storage;
        private readonly NavigationManager _navigation;
        public ApiDataProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILocalStorageService storage, NavigationManager navigation)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _storage = storage;
            _navigation = navigation;

            Url = _configuration["ServerIP"];
            TimeOut = TimeSpan.FromSeconds(20);
        }
        #region 设置信息
        public string Url { get; set; }
        public TimeSpan TimeOut { get; set; }
        private const string TOKEN = "Token";
        #endregion


        #region Token模式
        public async Task<Dictionary<string, string>> GetHeader()
        {
            var token = await _storage.GetItemAsStringAsync(TOKEN);

            Dictionary<string, string> header = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(token))
            {
                header.Add("Authorization", string.Format("Bearer {0}", token));
            }
            else
            {
                throw new Exception("No Token");
            }
            return header;
        }
        #endregion

        public async Task<AjaxResult<string>> GetToken(string userName, string password)
        {
            try
            {
                var content = await PostAsyncJson(string.Format("{0}/Base_Manage/Home/SubmitLogin", Url), (new { userName = userName, password = password }).ToJson(), TimeOut);
                var result = content.ToObject<AjaxResult<string>>();
                await _storage.SetItemAsStringAsync(TOKEN, result.Data);

                return result;
            }
            catch (Exception ex)
            {
                return new AjaxResult<string>() { Msg = ex.Message, Success = false };
            }

        }

        public async Task<AjaxResult> ClearToken()
        {
            await _storage.RemoveItemAsync(TOKEN);
            return new AjaxResult() { Success = true };
        }

        //[LoggerAttribute]
        public async Task<AjaxResult<T>> PostData<T>(string url, Dictionary<string, string> data)
        {
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = Url + url;
                }
                MultipartFormDataContent stringContent = null;
                if (data != null)
                {
                    stringContent = new MultipartFormDataContent();

                    foreach (var item in data)
                    {
                        stringContent.Add(new StringContent(item.Value), item.Key);
                    }
                }

                var content = await PostAsync(url, content: stringContent, TimeOut, await GetHeader());
                var result = content.ToObject<AjaxResult<T>>();
                return result;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigation.NavigateTo("/Home/Login");
                }
                return new AjaxResult<T>() { Msg = e.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new AjaxResult<T>() { Msg = ex.Message, Success = false };
            }
        }

        //[LoggerAttribute]
        public async Task<AjaxResult<T>> PostData<T>(string url, string json)
        {
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = Url + url;
                }
                var content = await PostAsyncJson(url, json, TimeOut, await GetHeader());

                var result = content.ToObject<AjaxResult<T>>();
                return result;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigation.NavigateTo("/Home/Login");
                }
                return new AjaxResult<T>() { Msg = e.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new AjaxResult<T>() { Msg = ex.Message, Success = false };
            }
        }

        //[LoggerAttribute]
        public async Task<AjaxResult<T>> PostData<T>(string url, object data)
        {
            return await PostData<T>(url, data.ToJson());
        }

        public async Task<AjaxResult<T>> GetData<T>(string url, Dictionary<string, string> data)
        {
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = Url + url;
                }

                var content = await GetAsync(url, TimeOut, data, await GetHeader());
                var result = content.ToObject<AjaxResult<T>>();
                return result;
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigation.NavigateTo("/Home/Login");
                }
                return new AjaxResult<T>() { Msg = e.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new AjaxResult<T>() { Msg = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyColumn">类型的主键，目前系统都是id</param>
        /// <param name="ids">需要删除的id列表</param>
        /// <returns></returns>
        public async Task<AjaxResult> UploadFile(string path, string fileName, string remark)
        {
            try
            {
                var data = new MultipartFormDataContent();
                ////添加字符串参数，参数名为qq
                //data.Add(new StringContent(qq), "qq");

                //添加文件参数，参数名为files，文件名为123.png
                data.Add(new ByteArrayContent(File.ReadAllBytes(path)), "file", fileName);

                var content = await PostAsync(string.Format("{0}/api/FileServer/SaveFile", Url), data, TimeOut, await GetHeader());
                var result = content.ToObject<AjaxResult<string>>();
                return result;
            }
            catch (Exception ex)
            {
                return new AjaxResult() { Msg = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyColumn">类型的主键，目前系统都是id</param>
        /// <param name="ids">需要删除的id列表</param>
        /// <returns></returns>
        public async Task<AjaxResult> DownLoadFile(string fullpath, string savepath)
        {
            try
            {
                FileStream fs = null;
                try
                {
                    var content = await GetByteArrayAsync(fullpath, TimeOut);
                    fs = new FileStream(savepath, FileMode.Create);
                    fs.Write(content, 0, content.Length);
                    return new AjaxResult() { Success = true };
                }
                catch (Exception ex)
                {
                    return new AjaxResult() { Success = false, Msg = ex.ToString() };
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
            catch (Exception ex)
            {
                return new AjaxResult() { Msg = ex.Message, Success = false };
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyColumn">类型的主键，目前系统都是id</param>
        /// <param name="ids">需要删除的id列表</param>
        /// <returns></returns>
        public async Task<UploadResult> UploadFileByForm(string path)
        {
            try
            {
                var data = new MultipartFormDataContent();

                FileStream fStream = File.Open(path, FileMode.Open, FileAccess.Read);
                data.Add(new StreamContent(fStream, (int)fStream.Length), "file", Path.GetFileName(path));

                var content = await PostAsync(string.Format("{0}/Base_Manage/Upload/UploadFileByForm", Url), data, TimeOut, await GetHeader());
                var result = content.ToObject<UploadResult>();

                fStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                return new UploadResult() { status = ex.Message };
            }
        }

        public async Task<UploadResult> UploadFileByForm(IBrowserFile file)
        {
            try
            {
                var data = new MultipartFormDataContent();

                Stream fStream = file.OpenReadStream();
                data.Add(new StreamContent(fStream, (int)file.Size), "file", file.Name);

                var content = await PostAsync(string.Format("{0}/Base_Manage/Upload/UploadFileByForm", Url), data, TimeOut, await GetHeader());
                var result = content.ToObject<UploadResult>();

                fStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                return new UploadResult() { status = ex.Message };
            }
        }


        #region HttpClient
        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsyncJson(string url, string json, TimeSpan timeSpan, Dictionary<string, string> header = null)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = timeSpan;
            HttpContent content = new StringContent(json);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            string responseBody = string.Empty;
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseBody;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsync(string url, HttpContent content, TimeSpan timeSpan, Dictionary<string, string> header = null)
        {
            //HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = timeSpan;
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            string responseBody = string.Empty;
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseBody;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> PostAsync(string url, string data, TimeSpan timeSpan, Dictionary<string, string> header = null)
        {
            //HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = timeSpan;
            HttpContent content = new StringContent(data);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            string responseBody = string.Empty;
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseBody;
        }

        public async Task<byte[]> GetByteArrayAsync(string uri, TimeSpan timeSpan, Dictionary<string, string> header = null)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = timeSpan;
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            byte[] urlContents = await client.GetByteArrayAsync(uri);
            return urlContents;
        }

        /// <summary>
        /// 使用get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <returns>返回的字符串</returns>
        public async Task<string> GetAsync(string url, TimeSpan timeSpan, Dictionary<string, string> data, Dictionary<string, string> header = null)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (data.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in data)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }

            HttpClient client = _httpClientFactory.CreateClient();
            client.Timeout = timeSpan;
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            string responseBody = string.Empty;

            try
            {
                HttpResponseMessage response = await client.GetAsync(builder.ToString());
                response.EnsureSuccessStatusCode();//用来抛异常的
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseBody;
        }
        #endregion
    }
}
