using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                var inputJson = File.ReadAllText("./../../../Datasets/sales.json");

                var result = GetTotalSalesByCustomer(context);

                Console.WriteLine(result);
            }
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var salesData = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(y=>y.Car.PartCars.Sum(m=>m.Part.Price))
                })
                .OrderByDescending(x=>x.SpentMoney)
                .ThenByDescending(x=>x.BoughtCars)
                .ToList();


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            };

            var jsonProduct = JsonConvert.SerializeObject(salesData, newSettings);

            return jsonProduct;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(x => new
                {
                    car = new
                    {
                        x.Make,
                        x.Model,
                        x.TravelledDistance,
                        parts = x.PartCars.Select(y => new
                        {
                            Name = y.Part.Name,
                            Price = y.Part.Price
                        })
                    }
                })
                .ToList();


            var newSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };

            var jsonProduct = JsonConvert.SerializeObject(cars, newSettings);

            return jsonProduct;
        }


        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            var newSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            var jsonProduct = JsonConvert.SerializeObject(suppliers, newSettings);

            return jsonProduct;
        }


        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .ToList();


            var newSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            var jsonProduct = JsonConvert.SerializeObject(cars, newSettings);

            return jsonProduct;

        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x =>
                new { x.Name, x.BirthDate, x.IsYoungDriver })
                .ToList();

            var newSettings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
                Formatting = Formatting.Indented
            };

            var jsonProduct = JsonConvert.SerializeObject(customers, newSettings);

            return jsonProduct;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            var addedSales = context.SaveChanges();

            var result = $"Successfully imported {addedSales}.";

            return result;
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd hh:mm:ss"
            });

            context.Customers.AddRange(customers);

            var addedEntries = context.SaveChanges();


            var result = $"Successfully imported {addedEntries}.";

            return result;
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDTOs = JsonConvert.DeserializeObject<List<CarDTO>>(inputJson);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = new Mapper(config);

            foreach (var carDTO in carDTOs)
            {
                var carToAdd = mapper.Map<CarDTO, Car>(carDTO);

                foreach (var partId in carDTO.PartsId.Distinct())
                {
                    var partCarEntity = new PartCar()
                    {
                        PartId = partId,
                        Car = carToAdd
                    };

                    context.PartCars.Add(partCarEntity);
                }
            }

            context.SaveChanges();

            var result = $"Successfully imported {carDTOs.Count}.";

            return result;
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            var partsToAdd = parts.Where(x => suppliersIds.Contains(x.SupplierId));

            context.AddRange(partsToAdd);

            var partsCount = context.SaveChanges();

            var result = $"Successfully imported {partsCount}.";

            return result;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            var suppliersCount = context.SaveChanges();

            var result = $"Successfully imported {suppliersCount}.";

            return result;
        }
    }
}