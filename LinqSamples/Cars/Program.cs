using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            //            var query = cars.OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name);
            var query =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2016
                orderby car.Combined descending, car.Name
                select new
                //                {
                //                    Manufacturer = car.Manufacturer,
                //                    Name = car.Name,
                //                    Cylinders = car.Cylinders,
                //                    Combined = car.Combined
                //                };
                {
                    car.Manufacturer,
                    car.Name,
                    car.Cylinders,
                    car.Combined
                };

            //            var result = cars.Select(c => new {c.Manufacturer, c.Name, c.Combined});
            var result = cars.SelectMany(c => c.Name)
                .OrderBy(c => c);

            foreach (var character in result)
            {
                Console.WriteLine(character);
            }

            //            var anonymous = new
            //            {
            //                Name = "Marcelo"
            //            };
            //            Console.WriteLine(anonymous.Name);

            //            var check = cars.Any();
            //            var check = cars.Any(c => c.Manufacturer == "Ford");
            //            var check = cars.All(c => c.Manufacturer == "Ford");
            //            var check = cars.Contains(c => c.Manufacturer == "Ford"); // conflict with List.Contains, but would work if the class does not have the method.

            //            Console.WriteLine(check);

            //            var top =
            //                cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //                    .OrderByDescending(c => c.Combined)
            //                    .ThenBy(c => c.Name)
            //                    .First();
            //            Console.WriteLine(top.Name);


            //            var query2 =
            //                cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //                    .OrderByDescending(c => c.Combined)
            //                    .ThenBy(c => c.Name);

            //            foreach (var car in query.Take(10))
            //            {
            //                Console.WriteLine($"{car.Manufacturer} {car.Name}, {car.Cylinders} cylinders : {car.Combined}");
            //            }

            //            Console.WriteLine("***");
            //            foreach (var car in query2.Take(10))
            //            {
            //                Console.WriteLine($"{car.Manufacturer} {car.Name}, {car.Cylinders} cylinders : {car.Combined}");
            //            }

        }

        private static List<Car> ProcessFile(string path)
        {
            //            return
            //            File.ReadAllLines(path)
            //                .Skip(1) // Skip header line
            //                .Where(line => line.Length > 1) // ignore empty lines
            //                .Select(Car.ParseFromCsv)
            //                .ToList();

            //            (
            //                from line in File.ReadAllLines(path).Skip(1)
            //                where line.Length > 1
            //                select Car.ParseFromCsv(line)
            //            ).ToList();

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
}