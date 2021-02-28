using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.LoginLog.Output;
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
    public class LoginLogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public LoginLogService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<ResponseModel<PageOutput<LoginLogListOutput>>> GetLoginLogsAsync(PageInput<LoginLogEntity> model)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("bearer",token);
            var response = await _httpClient.PostAsJsonAsync("api/Admin/LoginLog/GetPage", model);
            return await response.ResponseModel<PageOutput<LoginLogListOutput>>();
        }
    }
}
