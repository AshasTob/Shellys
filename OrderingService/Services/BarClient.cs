using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OrderingService.Services
{
    public class BarClient
    {
        // _httpClient isn't exposed publicly
        private readonly HttpClient _httpClient;

        public BarClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IEnumerable<MenuItem>> GetMenu()
        {
            var response = await _httpClient.GetAsync("api/menu");

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<MenuItem> GetMenuItem(string name)
        {
            var response = await _httpClient.GetAsync($"api/menu/{name}");

            response.EnsureSuccessStatusCode();
            return  JsonConvert.DeserializeObject<MenuItem>(await response.Content.ReadAsStringAsync());
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
