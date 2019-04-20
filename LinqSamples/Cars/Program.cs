using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");
            var query =
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ?? Enumerable.Empty<XElement>()
                    //                from element in document.Descendants("Car")
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;
            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
            //            var query = from element in document.Element("Cars").Elements("Car");

        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

            var document = new XDocument();
            //            var cars = new XElement("Cars");
            //
            //            var elements = 
            //                from record in records
            //                select  new XElement("Car",
            //                    new XAttribute("Name", record.Name),
            //                    new XAttribute("Combined", record.Combined),
            //                    new XAttribute("Manufacturer", record.Manufacturer));
            //            cars.Add(elements);

            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Combined", record.Combined),
                    new XAttribute("Manufacturer", record.Manufacturer)));
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            //            foreach (var record in records)
            //            {
            //                //                var car = new XElement("Car");
            //                //                var name = new XElement("Name", record.Name);
            //                //                var combined = new XElement("Combined", record.Combined);
            //                //                var name = new XAttribute("Name", record.Name);
            //                //                var combined = new XAttribute("Combined", record.Combined);
            //
            //                //                car.Add(name);
            //                //                car.Add(combined);
            //
            //                var car = new XElement("Car",
            //                    new XAttribute("Name", record.Name),
            //                    new XAttribute("Combined", record.Combined),
            //                    new XAttribute("Manufacturer", record.Manufacturer));
            //                cars.Add(car);
            //            }
            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });
            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToCar();
            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = int.MinValue;
            Min = int.MaxValue;
            Average = 0;
            Total = 0;
            Count = 0;
        }
        public int Max { get; set; }
        public int Min { get; set; }
        public int Average { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }

        public CarStatistics Accumulate(Car car)
        {
            Total += car.Combined;
            Count++;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }
    }
}
