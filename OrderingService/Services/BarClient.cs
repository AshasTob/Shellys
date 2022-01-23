using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<MenuItem>>(responseStream);
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
