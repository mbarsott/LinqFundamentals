using System;
using System.Collections.Generic;
using System.Linq;

//using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> square = i => i * i;
            Func<int, int, int> add = (x, y) =>
            {
                int temp = x + y;
                return temp;
            };
            Action<int> write = i => Console.WriteLine(i);
            write(square(add(3, 5)));

            //            Employee[] developers = new Employee[]
            //            IEnumerable<Employee> developers = new Employee[]
            var developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott"},
                new Employee {Id = 2, Name = "Chris"}
            };

            //            List<Employee> sales = new List<Employee>
            //            IEnumerable<Employee> sales = new List<Employee>
            var sales = new List<Employee>
            {
                new Employee {Id = 3, Name = "Alex"}
            };

            //            Console.WriteLine(MyLinq.Count(developers));
            //            Console.WriteLine(developers.Count());
            //            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            //            while (enumerator.MoveNext())
            //            {
            //                Console.WriteLine(enumerator.Current.Name);
            //            }

            //            foreach (var employee in developers.Where(NameStartsWithS))
            //            foreach (var employee in developers.Where(delegate (Employee arg)
            //                {
            //                    return arg.Name.StartsWith("S");
            //                }))
            //            foreach (var employee in developers.Where(e => e.Name.StartsWith("S")))
            //            foreach (var employee in developers.Where(e => e.Name.Length == 5)
            //            var query = developers.Where(e => e.Name.Length == 5).OrderBy(e => e.Name);
            var query = developers.Where(e => e.Name.Length == 5).OrderBy(e => e.Name).Select(e => e);

            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name
                         select developer;

            foreach (var employee in query2)
            {
                Console.WriteLine(employee.Name);
            }

        }

        //        private static int Square(int arg)
        //        {
        //            return arg * arg; 
        //        }

        private static bool NameStartsWithS(Employee arg)
        {
            return arg.Name.StartsWith("S");
        }
    }
}
