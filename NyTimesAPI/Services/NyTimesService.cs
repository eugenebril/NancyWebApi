using NyTimesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NyTimesApi.Options;

namespace NyTimesApi.Services
{
    public class NyTimesService : INyTimesService
    {
        private const string API_V2 = "svc/topstories/v2";

        private readonly HttpClient _httpClient;
        private readonly NytSettings _nytSettings;

        public NyTimesService(HttpClient httpClient, NytSettings nytSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _nytSettings = nytSettings ?? throw new ArgumentNullException(nameof(nytSettings));
            _httpClient.BaseAddress = new Uri(_nytSettings.BaseUrl);
        }

        public async Task<IEnumerable<Article>> GetArticlesBySectionAsync(string section)
        {
            var response = await _httpClient.GetAsync($"{API_V2}/{section}.json?api-key={_nytSettings.ApiKey}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var resultWrapper = JsonConvert.DeserializeObject<TopStoriesResponse>(data);
            return resultWrapper?.Articles ?? Enumerable.Empty<Article>();
        }
    }
}
