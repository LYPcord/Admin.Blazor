using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Admin.Blazor.Shared.Data.Admin.Models;
using Admin.Core.Service.Admin.Auth.Input;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Admin.Core;

namespace Admin.Blazor.Shared.Data.Admin
{
    public class AuthService 
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<ResponseModel<string>> LoginAsync(AuthLoginInput input)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Admin/Auth/Login", input);

            var result = await response.ResponseJson();

            if (result.Code == 1) 
            {
                string token = result.Data.GetValueByKey("token");

                await _localStorage.SetItemAsync("authToken", token);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
            
            return result;
        }

        public async Task<ResponseModel<UserInfo>> GetUserInfoAsync() 
        {
            var response = await _httpClient.GetAsync("api/Admin/Auth/GetUserInfo");
            return await response.ResponseModel<UserInfo>();
        }
    }
}
