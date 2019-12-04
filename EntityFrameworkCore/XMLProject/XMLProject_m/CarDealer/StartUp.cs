using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                string xmlString = System.IO.File.ReadAllText("./../../../Datasets/sales.xml");

                var result = GetSalesWithAppliedDiscount(context);

                Console.WriteLine(result);
            }

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sale = context.Sales
                .Select(x => new SaleExportDTO()
                {
                    Car = new CarWithDistanceAttrDTO
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },

                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = string.Format("{0}", x.Car.PartCars.Sum(y => y.Part.Price)),
                    PriceWithDiscount = string.Format("{0}", x.Car.PartCars.Sum(y => y.Part.Price) * (1 - x.Discount / 100))
                })
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<SaleExportDTO>),
                new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, sale, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new CustomerCarDTO()
                {
                    Name = x.Name,
                    BoughtCarsCount = x.Sales.Count,
                    TotalMoneySpent = x.Sales.Sum(m => m.Car.PartCars.Sum(l => l.Part.Price))
                })
                .OrderByDescending(x => x.TotalMoneySpent)
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<Dtos.Export.CustomerCarDTO>),
                new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new CarPartsDTO()
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars
                    .Select(y => new Dtos.Export.PartDTO()
                    {
                        Name = y.Part.Name,
                        Price = y.Part.Price
                    })
                    .OrderByDescending(y => y.Price)
                    .ToList()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<Dtos.Export.CarPartsDTO>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new Dtos.Export.SupplierDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<Dtos.Export.SupplierDTO>),
                new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, suppliers, namespaces);
            }

            return sb.ToString();

        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new CarBMWDTO()
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<CarBMWDTO>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .OrderBy(y => y.Make)
                .ThenBy(y => y.Model)
                .Take(10)
                .Select(x => new CarWithDistanceDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<CarWithDistanceDTO>),
                new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SaleDTO>), new XmlRootAttribute("Sales"));

            var deserializedSales = new List<SaleDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedSales = (List<SaleDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var carIds = context.Cars.Select(x => x.Id).ToList();

            deserializedSales = deserializedSales.Where(x => carIds.Contains(x.CarId)).ToList();

            IMapper mapper = new Mapper(config);
            var sales = deserializedSales.Select(x => mapper.Map<SaleDTO, Sale>(x)).ToList();

            context.Sales.AddRange(sales);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CustomerDTO>), new XmlRootAttribute("Customers"));

            var deserializedCustomers = new List<CustomerDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedCustomers = (List<CustomerDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var customers = deserializedCustomers.Select(x => mapper.Map<CustomerDTO, Customer>(x)).ToList();

            context.Customers.AddRange(customers);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CarDTO>), new XmlRootAttribute("Cars"));

            var deserializedCars = new List<CarDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedCars = (List<CarDTO>)serializer.Deserialize(reader);
            }

            var partIds = context.Parts.Select(x => x.Id).ToList();

            foreach (var car in deserializedCars)
            {
                car.Parts.RemoveAll(x => !partIds.Contains(x.Id));
            }

            foreach (var car in deserializedCars)
            {
                var newList = new List<PartIdsDTO>();

                for (int i = 0; i < car.Parts.Count; i++)
                {
                    var id = car.Parts[i].Id;

                    if (!newList.Any(x => x.Id == id))
                    {
                        newList.Add(new PartIdsDTO() { Id = id });
                    }
                }

                car.Parts = new List<PartIdsDTO>(newList);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var cars = deserializedCars.Select(x => mapper.Map<CarDTO, Car>(x)).ToList();

            var carsCollection = new List<Car>();
            var partsCollection = new List<PartCar>();

            foreach (var carDTO in deserializedCars)
            {
                var entity = mapper.Map<CarDTO, Car>(carDTO);
                carsCollection.Add(entity);

                foreach (var part in carDTO.Parts)
                {
                    var partCarEntity = new PartCar()
                    {
                        PartId = part.Id,
                        Car = entity
                    };

                    partsCollection.Add(partCarEntity);
                }
            }

            context.Cars.AddRange(carsCollection);
            context.PartCars.AddRange(partsCollection);

            context.SaveChanges();

            var result = $"Successfully imported {carsCollection.Count}";

            return result;
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<Dtos.Import.PartDTO>), new XmlRootAttribute("Parts"));

            var deserializedParts = new List<Dtos.Import.PartDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedParts = (List<Dtos.Import.PartDTO>)serializer.Deserialize(reader);
            }

            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            var partsToAdd = deserializedParts.Where(x => suppliersIds.Contains(x.SupplierId)).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var parts = partsToAdd.Select(x => mapper.Map<Dtos.Import.PartDTO, Part>(x)).ToList();

            context.Parts.AddRange(parts);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<Dtos.Import.SupplierDTO>), new XmlRootAttribute("Suppliers"));

            var deserializedSuppliers = new List<Dtos.Import.SupplierDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedSuppliers = (List<Dtos.Import.SupplierDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var suppliers = deserializedSuppliers.Select(x => mapper.Map<Dtos.Import.SupplierDTO, Supplier>(x)).ToList();

            context.Suppliers.AddRange(suppliers);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }
    }
}