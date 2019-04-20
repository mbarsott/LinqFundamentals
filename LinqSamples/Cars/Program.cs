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
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query =
            //                from car in cars
            //                join manufacturer in manufacturers on car.Manufacturer equals manufacturer.Name
            //                orderby car.Combined descending, car.Name ascending
            //                select new
            //                {
            //                    manufacturer.Headquarters,
            //                    car.Name,
            //                    car.Combined
            //                };

            //                cars.Join(manufacturers,
            //                        c => c.Manufacturer,
            //                        m => m.Name,
            //                        (c, m) => new
            //                        {
            //                            m.Headquarters,
            //                            c.Name,
            //                            c.Combined
            //                        })
            //                    .OrderByDescending(t => t.Combined)
            //                    .ThenBy(t => t.Name);

            //                cars.Join(manufacturers,
            //                        c => c.Manufacturer,
            //                        m => m.Name,
            //                        (c, m) => new
            //                        {
            //                            Car = c,
            //                            Manufacturer = m
            //                        })
            //                    .OrderByDescending(c => c.Car.Combined)
            //                    .ThenBy(c => c.Car.Name)
            //                    .Select(c => new
            //                    {
            //                        c.Manufacturer.Headquarters,
            //                        c.Car.Name,
            //                        c.Car.Combined
            //                    });


            //            from car in cars
            //            join manufacturer in manufacturers
            //                on new { car.Manufacturer, car.Year } equals new { Manufacturer = manufacturer.Name, manufacturer.Year }
            //            orderby car.Combined descending, car.Name ascending
            //            select new
            //            {
            //                manufacturer.Headquarters,
            //                car.Name,
            //                car.Combined
            //            };

            cars.Join(manufacturers,
                    c => new { c.Manufacturer, c.Year },
                    m => new { Manufacturer = m.Name, m.Year },
                    (c, m) => new
                    {
                        m.Headquarters,
                        c.Name,
                        c.Combined
                    })
                .OrderByDescending(t => t.Combined)
                .ThenBy(t => t.Name);

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            }

            Console.WriteLine("***");

            var query2 =
                //                from car in cars
                //                group car by car.Manufacturer.ToUpper() into manufacturer
                //                orderby manufacturer.Key
                //                select manufacturer;

                //                cars.GroupBy(c => c.Manufacturer.ToUpper())
                //                    .OrderBy(m => m.Key);

                //            foreach (var group in query2)
                //            {
                //                Console.WriteLine(group.Key);
                //                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                //                {
                //                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                //                }
                //            }

                //                from manufacturer in manufacturers
                //                join car in cars
                //                    on manufacturer.Name equals car.Manufacturer into carGroup
                //                orderby manufacturer.Name
                //                select new
                //                {
                //                    Manufacturer = manufacturer,
                //                    Cars = carGroup
                //                };

                //                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                //                        new
                //                        {
                //                            Manufacturer = m,
                //                            Cars = g
                //                        })
                //                    .OrderBy(m => m.Manufacturer.Name);
                //
                //            foreach (var group in query2)
                //            {
                //                Console.WriteLine($"{group.Manufacturer.Name} - {group.Manufacturer.Headquarters}");
                //                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                //                {
                //                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                //                }
                //            }

                //                from manufacturer in manufacturers
                //                join car in cars
                //                    on manufacturer.Name equals car.Manufacturer into carGroup
                //                orderby manufacturer.Name
                //                select new
                //                {
                //                    Manufacturer = manufacturer,
                //                    Cars = carGroup
                //                } into result
                //                group result by result.Manufacturer.Headquarters;

                //                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
                //                        (m, g) =>
                //                            new
                //                            {
                //                                Manufacturer = m,
                //                                Cars = g
                //                            })
                //                    .GroupBy(m => m.Manufacturer.Headquarters);

                //
                //            foreach (var group in query2)
                //            {
                //                Console.WriteLine($"{group.Key}");
                //                foreach (var car in group.SelectMany(g => g.Cars)
                //                    .OrderByDescending(c => c.Combined).Take(3))
                //                {
                //                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                //                }
                //            }

                //            from car in cars
                //            group car by car.Manufacturer into carGroup
                //            select new
                //            {
                //                Name = carGroup.Key,
                //                Max = carGroup.Max(c => c.Combined),
                //                Min = carGroup.Min(c => c.Combined),
                //                Avg = carGroup.Average(c => c.Combined)
                //            } into result
                //            orderby result.Max descending
                //            select result;

                cars.GroupBy(c => c.Manufacturer)
                    .Select(g =>
                    {
                        var results = g.Aggregate(new CarStatistics(),
                                (acc, c) => acc.Accumulate(c),
                            acc => acc.Compute());
                        return new
                        {
                            Name = g.Key,
                            Avg = results.Average,
                            Max = results.Max,
                            Min = results.Min
                        };
                    });

            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\tMax: {result.Max}");
                Console.WriteLine($"\tMin: {result.Min}");
                Console.WriteLine($"\tAvg: {result.Avg}");
            }
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
