using Admin.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data
{
    public class HttpHelpers
    {
        private readonly HttpClient _httpClient;
        public HttpHelpers(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<string>> PostAsJsonAsync(string url,object param)
        {
            var response = await _httpClient.PostAsJsonAsync(url, param);
            response.EnsureSuccessStatusCode();
            var rm = await response.Content.ReadFromJsonAsync<ResponseModel>();
            if (rm.Code == 1)
            {
                return new ResponseModel<string>()
                {
                    Code = rm.Code,
                    Msg = rm.Msg,
                    Data = rm.Data.ToString()
                };
            }
            return new ResponseModel<string>()
            {
                Code = rm.Code,
                Msg = rm.Msg
            };
        }

        public async Task<ResponseModel<T>> PostAsJsonAsync<T>(string url, object param)
        {
            var response = await _httpClient.PostAsJsonAsync(url, param);
            response.EnsureSuccessStatusCode();
            var rm = await response.Content.ReadFromJsonAsync<ResponseModel>();
            if (rm.Code == 1)
            {
                return new ResponseModel<T>()
                {
                    Code = rm.Code,
                    Msg = rm.Msg,
                    Data = rm.Data.ToString().ToObject<T>()
                };
            }
            return new ResponseModel<T>()
            {
                Code = rm.Code,
                Msg = rm.Msg
            };
        }
    }
}
