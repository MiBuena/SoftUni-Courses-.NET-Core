using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSON
{
    public class ProductDTO
    {
        [JsonProperty("TheName")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int? SellerId { get; set; }

        public int? BuyerId { get; set; }

        [JsonIgnore]
        public string M { get; set; }
    }
}
