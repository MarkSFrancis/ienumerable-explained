using System;
using System.Collections.Generic;
using System.Linq;

namespace Explained
{
    using static Console;

    internal class Program
    {
        private static void Main(string[] args)
        {
            WriteLine("All names:");
            WriteAllNames();

            if (!Continue()) return;

            WriteLine("Names with an \"A\" in:");
            WriteAllNamesWithAIn();

            if (!Continue()) return;

            WriteLine("All names in alphabetical order:");
            WriteAllNamesInOrder();

            if (!Continue()) return;

            WriteLine("Numbers from 1 to 20 that are even:");
            WriteSomeEvenNumbers();

            WriteLine("Press enter to exit");
            WaitForKeyPress(ConsoleKey.Enter);
        }

        private static void WriteAllNames()
        {
            foreach (var name in Generator.GetNames())
            {
                WriteLine(name);
            }
        }

        private static void WriteSomeEvenNumbers()
        {
            IEnumerable<int> allNumbers = Generator.GetRange(1, 20);
            IEnumerable<int> evenNumbers = LinqService.FilterEvenNumbers(allNumbers);

            foreach (var number in evenNumbers)
            {
                WriteLine(number);
            }
        }

        private static void WriteAllNamesWithAIn()
        {
            var names = Generator.GetNames();

            IEnumerable<string> namesThatHaveA = LinqService.FilterTextBy(names, 
                n => n.Contains("a", StringComparison.InvariantCultureIgnoreCase));

            foreach (var name in namesThatHaveA)
            {
                WriteLine(name);
            }
        }

        private static void WriteAllNamesInOrder()
        {
            var allNames = Generator.GetNames();

            var orderedNames = LinqService.InAlphabeticalOrder(allNames);

            foreach(var name in orderedNames)
            {
                WriteLine(name);
            }
        }

        private static ConsoleKey WaitForKeyPress(params ConsoleKey[] keys)
        {
            ConsoleKeyInfo keyPress;

            do
            {
                keyPress = ReadKey(true);
            } while (!keys.Contains(keyPress.Key));

            WriteLine(keyPress.KeyChar);
            return keyPress.Key;
        }

        private static bool Continue()
        {
            WriteLine($"Press {nameof(ConsoleKey.Escape)} to exit, or {nameof(ConsoleKey.Enter)} to continue");

            var keyPress = WaitForKeyPress(ConsoleKey.Escape, ConsoleKey.Enter);
            WriteLine();

            return keyPress == ConsoleKey.Enter;
        }
    }
}
