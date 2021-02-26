using Admin.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Admin.Blazor.Shared.Data
{
    public static class HttpResponseMessageExpansion
    {

        public static async Task<ResponseModel<string>> ResponseJson(this HttpResponseMessage response)
        {
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

        public static async Task<ResponseModel<T>> ResponseObject<T>(this HttpResponseMessage response)
        {
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
