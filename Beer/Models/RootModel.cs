using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beer.Models
{
    public class RootModel
    {
        public int currentPage { get; set; }
        public int numberOfPages { get; set; }
        public int totalResults { get; set; }
        [JsonProperty("data")]
        public List<BeerModel> data { get; set; }
        public string status { get; set; }
    }
}
