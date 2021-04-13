using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NancyWebApi.Models
{
    public class ArticleView
    {
        [JsonProperty("heading")] 
        public string Heading { get; set; }

        [JsonProperty("updated")] 
        public DateTime Updated { get; set; }

        [JsonProperty("link")] 
        public string Link { get; set; }
    }
}
