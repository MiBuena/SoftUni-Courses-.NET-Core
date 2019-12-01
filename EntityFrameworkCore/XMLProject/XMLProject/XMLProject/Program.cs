using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XMLProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //XDocument document = XDocument.Load("./../../../Resources/cars.xml");

            //XElement root = document.Root;

            //var cars = root.Elements();

            //foreach (var car in cars)
            //{
            //    string make = car.Element("make").Value;

            //    string model = car.Element("model").Value;

            //    Console.WriteLine($"{make} {model}");
            //}


            var books = new List<Book>()
            {
                new Book("First book", "First author"),
                new Book("Second book", "Second author")
            };

            var serializer = new XmlSerializer(typeof(List<Book>));

            using (var writer = new StreamWriter("./../../../Resources/myProduct.xml"))
            {
                serializer.Serialize(writer, books);
            }


        }
    }
}
