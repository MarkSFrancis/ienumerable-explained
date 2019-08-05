using System;
using System.Collections.Generic;
using System.Linq;

namespace Explained
{
    public static class LinqService
    {
        public static IEnumerable<int> FilterEvenNumbers(IEnumerable<int> numbers)
        {
            foreach (var number in numbers)
            {
                if (number % 2 == 0)
                {
                    yield return number;
                }
            }
        }

        public static IEnumerable<string> FilterNotWhitespace(IEnumerable<string> texts)
        {
            foreach (var text in texts)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    yield return text;
                }
            }
        }

        public static IEnumerable<string> FilterTextBy(IEnumerable<string> texts, Func<string, bool> shouldKeep)
        {
            foreach (var text in texts)
            {
                if (shouldKeep(text))
                {
                    yield return text;
                }
            }
        }

        public static IEnumerable<string> InAlphabeticalOrder(this IEnumerable<string> texts, bool ascending = true)
        {
            if (ascending)
            {
                return texts.OrderBy(t => t);
            }
            else
            {
                return texts.OrderByDescending(t => t);
            }
        }
    }
}
