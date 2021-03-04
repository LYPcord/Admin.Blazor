using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Role.Input;
using Admin.Core.Service.Admin.Role.Output;
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
    public class RoleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public RoleService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<RoleGetOutput>> GetRoleByIdAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Role/Get?id={id}");
            return await response.ResponseModel<RoleGetOutput>();
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ResponseModel<PageInput<RoleEntity>>> GetPageRoleListAsync(PageInput<RoleEntity> input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/Role/GetPage", input);
            return await response.ResponseModel<PageInput<RoleEntity>>();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ResponseModel> RoleAddAsync(RoleAddInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"api/Admin/Role/Add", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ResponseModel> RoleUpdateAsync(RoleUpdateInput input)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"api/Admin/Role/Update", input);
            return await response.ResponseModel();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel> RoleSoftDeleteAsync(long id)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Admin/Role/SoftDelete?id={id}");
            return await response.ResponseModel();
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResponseModel> RoleBatchSoftDeleteAsync(long[] ids)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"api/Admin/Role/BatchSoftDelete", ids);
            return await response.ResponseModel();
        }
    }
}
