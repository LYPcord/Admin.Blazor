﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.View.Input;
using Admin.Core.Service.Admin.View.Output;
using Blazored.LocalStorage;

namespace Admin.Blazor.Shared.Data.Admin
{
    public class ViewService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ViewService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        /// <summary>
        /// 查询单条视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ViewGetOutput>> GetViewByIdAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/View/Get?id={id}");
            return await response.ResponseModel<ViewGetOutput>();
        }

        /// <summary>
        /// 查询全部视图
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ViewGetOutput>> GetViewListAsync(string key)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/View/GetList?key={key}");
            return await response.ResponseModel<ViewGetOutput>();
        }

        /// <summary>
        /// 查询全部视图
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ViewEntity>> GetPageViewListAsync(PageInput<ViewEntity> input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/View/GetPage", input);
            return await response.ResponseModel<ViewEntity>();
        }

        /// <summary>
        /// 新增视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ViewAddAsync(ViewAddInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/View/Add", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ViewUpdateAsync(ViewUpdateInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"api/Admin/View/Update", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ViewSoftDeleteAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Admin/View/SoftDelete?id={id}");
            return await response.ResponseModel();
        }

        /// <summary>
        /// 批量删除视图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ViewBatchSoftDeleteAsync(long[] ids)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"api/Admin/View/BatchSoftDelete", ids);
            return await response.ResponseModel();
        }

        // <summary>
        /// 同步视图
        /// 支持新增和修改视图
        /// 根据视图是否存在自动禁用和启用视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseModel> ViewBatchSoftDeleteAsync(ViewSyncInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/View/Sync", input);
            return await response.ResponseModel();
        }
    }
}
