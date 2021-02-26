using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Admin.Core.Service.Admin.Auth.Input;

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
            return response.Response();
        }   
    }
}
