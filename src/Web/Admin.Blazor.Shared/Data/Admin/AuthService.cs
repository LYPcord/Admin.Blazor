using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Admin.Core.Service.Admin.Auth.Input;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace Admin.Blazor.Shared.Data.Admin
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<string>> LoginAsync(AuthLoginInput input)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Admin/Auth/Login", input);
            response.EnsureSuccessStatusCode();
            var rm = await response.Content.ReadFromJsonAsync<ResponseModel>();
            if (rm.Code==1)
            {
                string ss = rm.Data.ToString();
            }
            return "";
        }   
    }
}
