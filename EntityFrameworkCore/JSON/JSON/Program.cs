using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputJson = File.ReadAllText("./../../../products.json");

            var products = JsonConvert.DeserializeObject<List<ProductDTO>>(inputJson);


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new KebabCaseNamingStrategy()
            };

            var newSettings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                NullValueHandling = NullValueHandling.Ignore
            };

            var jsonProduct = JsonConvert.SerializeObject(products, newSettings);

            Console.WriteLine(jsonProduct);

        }
    }
}
