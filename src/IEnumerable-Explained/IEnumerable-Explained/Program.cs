using System;
using System.Collections.Generic;
using System.Linq;

namespace Explained
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WriteAllNames();

            WriteSomeEvenNumbers();
        }

        private static void WriteAllNames()
        {
            foreach (var name in Generator.GetNames())
            {
                Console.WriteLine(name);
            }
        }

        private static void WriteSomeEvenNumbers()
        {
            var allNumbers = Generator.GetRange(1, 20);
            var evenNumbers = LinqService.FilterEvenNumbers(allNumbers);

            foreach (var number in evenNumbers)
            {
                Console.WriteLine(number);
            }
        }

        private static void WriteAllNamesWithAIn()
        {

            var evenNumbers = LinqService.FilterEvenNumbers(allNumbers);

            foreach (var number in evenNumbers)
            {
                Console.WriteLine(number);
            }
        }
    }
}
