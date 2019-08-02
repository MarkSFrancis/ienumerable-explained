using System.Collections.Generic;

namespace Explained
{
    public static class Generator
    {
        public static IEnumerable<int> GetRange(int from, int total)
        {
            for (var index = 0; index < total; index++)
            {
                yield return from + index;
            }
        }

        public static IEnumerable<string> GetNames()
        {
            yield return "Steve";
            yield return "John";
            yield return "Sarah";
            yield return "David";
            yield return "Olivia";
        }
    }
}
