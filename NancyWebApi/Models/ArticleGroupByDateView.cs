using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NancyWebApi.Models
{
    public class ArticleGroupByDateView
    {
        [JsonProperty("date")] 
        public string Date { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
