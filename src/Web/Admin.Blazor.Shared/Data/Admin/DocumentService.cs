using Admin.Blazor.Shared.Data.Admin.Models;
using Admin.Core.Service.Admin.Document.Input;
using Admin.Core.Service.Admin.Document.Output;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data.Admin
{
    public class DocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public DocumentService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        /// <summary>
        /// 查询文档列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<ResponseModel<DocumentListOutput>> GetDocumentListAsync(string key, DateTime? start, DateTime? end)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Document/GetList?key={key}&start={start}&end={end}");
            return await response.ResponseModel<DocumentListOutput>();
        }

        /// <summary>
        /// 查询文档图片列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<List<string>>> GetImageListAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Document/GetImageList?id={id}");
            return await response.ResponseModel<List<string>>();
        }

        /// <summary>
        /// 查询单条分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<DocumentGetGroupOutput>> GetDocumentGetGroupByIdAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Document/GetGroup?id={id}");
            return await response.ResponseModel<DocumentGetGroupOutput>();
        }

        /// <summary>
        /// 查询单条菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<DocumentGetMenuOutput>> GetDocumentGetMenuByIdAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Document/GetMenu?id={id}");
            return await response.ResponseModel<DocumentGetMenuOutput>();
        }

        /// <summary>
        /// 查询单条文档内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<DocumentGetContentOutput>> GetDocumentGetContentByIdAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Document/GetContent?id={id}");
            return await response.ResponseModel<DocumentGetContentOutput>();
        }

        /// <summary>
        /// 查询精简文档列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<DocumentInfo>> GetPlainListAsync()
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync("api/Admin/Document/GetPlainList");
            return await response.ResponseModel<DocumentInfo>();
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentAddGroupAsync(DocumentAddGroupInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync("api/Admin/Document/AddGroup", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentAddMenuAsync(DocumentAddMenuInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync("api/Admin/Document/AddMenu", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentUpdateGroupAsync(DocumentUpdateGroupInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync("api/Admin/Document/UpdateGroup", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentUpdateMenuAsync(DocumentUpdateMenuInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync("api/Admin/Document/UpdateMenu", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 修改文档内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentUpdateContentAsync(DocumentUpdateContentInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync("api/Admin/Document/UpdateContent", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DocumentSoftDeleteAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Admin/Document/SoftDelete?id={id}");
            return await response.ResponseModel();
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DeleteImage(long documentId, string url)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Admin/Document/SoftDelete?documentId={documentId}&url={url}");
            return await response.ResponseModel();
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> UploadImage(DocumentUploadImageInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/Document/UploadImage", input);
            return await response.ResponseModel();
        }
    }
}
