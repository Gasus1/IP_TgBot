using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IP_TgBot
{
    public class IPDataClient
    {

        private HttpClient HttpClient;
        public static string _adress;
        public IPDataClient()
        {
            _adress = Constants.adress;
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(_adress);
        }
        public async Task<IP_Data> GetDataAsync(string query)
        {
            var response = await HttpClient.GetAsync($"IP?query={query}");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IP_Data>(content);
            return result;
        }
        public async Task<IP_Data> GetUserDataAsync()
        {
            var response = await HttpClient.GetAsync("GetUserData");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IP_Data>(content);
            return result;
        }
        public async Task PostUserDataAsync()
        {
            IP_Data data = await GetUserDataAsync();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync("UserIP", content).Result;
        }
        public async Task PostFoundIPDataAsync(string query)
        {
            IP_Data data = await GetDataAsync(query);
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync($"FoundIP?query={query}", content).Result;
        }
        public async Task<Country> GetCountryAsync()
        {
            var response = await HttpClient.GetAsync("Country");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Country>(content);
            return result;
        }
        public async Task<Query> GetQueryAsync()
        {
            var response = await HttpClient.GetAsync("Query");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Query>(content);
            return result;
        }
        public async Task<RegionName> GetRegionAsync()
        {
            var response = await HttpClient.GetAsync("Region");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<RegionName>(content);
            return result;
        }
        public async Task<City> GetCityAsync()
        {
            var response = await HttpClient.GetAsync("City");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<City>(content);
            return result;
        }
        public async Task<Coord> GetCoordAsync()
        {
            var response = await HttpClient.GetAsync("Coord");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Coord>(content);
            return result;
        }
        public async Task<Isp> GetIspAsync()
        {
            var response = await HttpClient.GetAsync("Isp");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Isp>(content);
            return result;
        }
        public async Task<Orgs> GetOrgsAsync()
        {
            var response = await HttpClient.GetAsync("Orgs");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Orgs>(content);
            return result;
        }
    }
}

