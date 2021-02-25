using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Admin.Core.Service.Admin.Auth.Input;

namespace Admin.Blazor.Shared.Data.Admin
{
    public class AuthService
    {
        private readonly HttpHelpers _httpClient;
        public AuthService(HttpHelpers httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<string>> LoginAsync(AuthLoginInput input)
        {
            return await _httpClient.PostAsJsonAsync("api/Admin/Auth/Login", input);
        }   
    }
}
