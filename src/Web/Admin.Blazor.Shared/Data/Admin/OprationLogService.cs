using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.OprationLog.Output;
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
    public class OprationLogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public OprationLogService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<ResponseModel<PageOutput<OprationLogListOutput>>> GetOprationLogsAsync(PageInput<OprationLogEntity> model)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsJsonAsync("api/Admin/OprationLog/GetPage", model);
            return await response.ResponseModel<PageOutput<OprationLogListOutput>>();
        }
    }
}
