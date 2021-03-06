using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NyTimesApi.Models
{
    public class Article
    {
        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("short_url")]
        public string ShortUrl { get; set; }
    }
}
