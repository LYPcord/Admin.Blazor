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
using Admin.Core.Service.Admin.Auth.Output;

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

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        public async Task<ResponseModel<AuthGetVerifyCodeOutput>> GetVerifyCodeBylastKeyAsnyc(string lastKey)
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Auth/GetVerifyCode?lastKey={lastKey}");
            return await response.ResponseModel<AuthGetVerifyCodeOutput>();
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GetPassWordEncryptKey()
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync($"api/Admin/Auth/GetPassWordEncryptKey");
            return await response.ResponseModel<string>();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<UserInfo>> GetUserInfoAsync()
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.GetAsync("api/Admin/Auth/GetUserInfo");
            return await response.ResponseModel<UserInfo>();
        }

        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ResponseModel<AuthLoginOutput>> RefreshAsync(string token)
        {
            string token2 = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token2);
            var response = await _httpClient.GetAsync($"api/Admin/Auth/Refresh?token={token}");
            return await response.ResponseModel<AuthLoginOutput>();
        }
    }
}
