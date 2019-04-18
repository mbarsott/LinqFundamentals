using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Windows";
            ShowLargeFilesWithoutLinq(path);
            Console.WriteLine("***");
            ShowLargeFilesWithLinq(path);
            Console.WriteLine("***");
            ShowLargeFilesWithLinqMethods(path);
        }

        private static void ShowLargeFilesWithLinqMethods(string path)
        {
            var query = new DirectoryInfo(path).GetFiles()
                .OrderByDescending(f => f.Length)
                .Take(5);
            foreach (var fileInfo in query)
            {
                Console.WriteLine($"{fileInfo.Name, -20} : {fileInfo.Length, 10:N0}");
            }
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            var query =
                from file in new DirectoryInfo(path).GetFiles()
                orderby file.Length descending
                select file;
            foreach (var fileInfo in query.Take(5))
            {
                Console.WriteLine($"{fileInfo.Name, -20} : {fileInfo.Length, 10:N0}");
            }
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            var files = directory.GetFiles();
            Array.Sort(files, new FileInfoComparer());
            for (int i = 0; i < 5 && i < files.Length; i++)
            {
                var fileInfo = files[i];
                Console.WriteLine($"{fileInfo.Name, -20} : {fileInfo.Length, 10:N0}");
            }
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}
