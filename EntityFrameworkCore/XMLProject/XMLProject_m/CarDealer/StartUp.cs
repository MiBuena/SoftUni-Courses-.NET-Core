using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                string xmlString = System.IO.File.ReadAllText("./../../../Datasets/customers.xml");

                var result = ImportCustomers(context, xmlString);
            }


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

                    if(!newList.Any(x=>x.Id == id))
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
            var serializer = new XmlSerializer(typeof(List<PartDTO>), new XmlRootAttribute("Parts"));

            var deserializedParts = new List<PartDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedParts = (List<PartDTO>)serializer.Deserialize(reader);
            }

            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            var partsToAdd = deserializedParts.Where(x => suppliersIds.Contains(x.SupplierId)).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var parts = partsToAdd.Select(x => mapper.Map<PartDTO, Part>(x)).ToList();

            context.Parts.AddRange(parts);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SupplierDTO>), new XmlRootAttribute("Suppliers"));

            var deserializedSuppliers = new List<SupplierDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedSuppliers = (List<SupplierDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });


            IMapper mapper = new Mapper(config);
            var suppliers = deserializedSuppliers.Select(x => mapper.Map<SupplierDTO, Supplier>(x)).ToList();

            context.Suppliers.AddRange(suppliers);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }
    }
}