using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Admin.BBApp.Shared.Models;
using Admin.Core.Service.Admin.Auth.Input;

namespace Admin.BBApp.Shared.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;
        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
        public async Task<Response<Token>> Login() 
        
        {
            var input = new AuthLoginInput();
            input.UserName = "admin";
            input.Password = "111111";
            var response =await _httpClient.PostAsJsonAsync("api/Admin/Auth/Login", input);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Response<Token>>();
        }

        public class Token 
        {
            public string token { get; set; }
        }
    }
}
