using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NyTimesApi.Models
{
    public class TopStoriesResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("num_results")]
        public int NumResults { get; set; }

        [JsonProperty("results")]
        public ICollection<Article> Articles { get; set; }
    }
}
