# C# - IEnumerable Explained

This solution contains examples of `LINQ` queries, `IEnumerable` with `yield`, and `IEnumerator` in use. You can clone this solution by clicking "Clone or Download` in the top right.

# The basics

## IEnumerable

`IEnumerable` is an interface that exposes only one method: `GetEnumerator()`.
```cs
public interface IEnumerable
{
    // Returns an IEnumerator for this enumerable Object.  The enumerator provides
    // a simple way to access all the contents of a collection.
    IEnumerator GetEnumerator();
}
```

## IEnumerator

`IEnumerator` is an interface that exposes 3 members:

```cs
public interface IEnumerator
{
    object Current { get; }
    
    bool MoveNext();
    
    void Reset();
}
```

## Using IEnumerable and IEnumerator

That `IEnumerator` iterates over a collection, such as an array or list, according to what the developer wants to iterate over. This `IEnumerator` is used by `foreach` when iterating over a collection. You can also iterate by hand, using the `IEnumerator` directly. The output of these two approaches are the same

For example, if you have a `List<string>` (which, in .NET, already implements `IEnumerable`), you can enumerate through each `string` in your list

```cs
List<string> names = new List<string>();
names.Add("Sam");
names.Add("David");

// Enumerating using foreach. This calls GetEnumerator(), MoveNext and Current for you
foreach (string name in names)
{
    Console.WriteLine(name);
}

// Output:
// Sam
// David

// Enumerating by hand
using (IEnumerator<string> namesEnumerator = names.GetEnumerator())
{
    // namesEnumerator.MoveNext() returns false when it reaches the end of the collection
    while (namesEnumerator.MoveNext())
    {
        Console.WriteLine(namesEnumerator.Current);
    }
}

// Output:
// Sam
// David
```

## What's LINQ?

LINQ stands for Language Integrated Query. It uses the features of an `IEnumerable` to allow you to query `IEnumerable` collections.

LINQ works on the premise that, because an IEnumerable is not enumerated until it's executed (such as in a `foreach`), operations can be stacked up, effectively queing them, and then ran all at once in a `foreach`. This allows you to run any number or combination of simple or complex queries on any collections

```cs
List<string> names = new List<string>();
names.Add("Sam");
names.Add("David");
names.Add("Sia");
names.Add("John");
names.Add("Sarah");

// Where is a LINQ operator. It filters a collection according to a given condition
// Get all names that start with "S"
IEnumerable<string> filteredNames = names.Where(n => n.StartsWith("S"));

// Take is another LINQ operator. It gets the first n results (in this example, the first 2 results)
// Get only the first 2 results of the filteredNames
filteredNames = filteredNames.Take(2);

// Enumerating executes both queries together, which means only the first two names that start with "S" will be printed
foreach (string name in filteredNames)
{
    Console.WriteLine(name);
}

// Output:
// Sam
// Sia
```

# The 3 stages

Combining IEnumerable and LINQ breaks down into 3 fundamental stages:

1. Obtain the data source, where the data is generated from an `IEnumerable` implementation
1. Create the query, where LINQ operators are stacked
1. Execute the query, when `foreach` is called

# Obtaining/ generating data

You can obtain `IEnumerable` data from virtually any source in .NET. If data is in the form of a collection, chances are, you can enumerate over it.

Some examples of collections include:
* `List<T>`
* `T[]` or `Array<T>`
* `Dictionary<TKey, TValue>`
* `IQueryable<T>` (from LINQ to SQL, Entity Framework, or a range of other ORMs)
* `XElement` (from LINQ to XML)
* `Queue<T>`
* `Stack<T>`
* `HashSet<T>`
* `LinkedList<T>`

## Using `yield return` and `yield break` to generate data

`yield` is a keyword in C#, which tells the compiler to return an item, and then when the thing that calls this function wants to continue, it continues where it left off within the method. This allows a method to generate data for `IEnumerable` collections

```cs
IEnumerable<string> GetNames()
{
    // Returns each item individually to the calling foreach code
    yield return "Sam";
    yield return "David";

    // Stops enumerating the collection
    yield break;

    // This line is never reached, as yield break is reached before this line is called
    yield return "Olivia";
}

IEnumerable<string> allNames = GetNames();
foreach (string name in allNames)
{
    Console.WriteLine(name);
}

// Output:
// Sam
// David
```

## Using `yield` to generate collections

Because of this feature where values can be yielded, collections can be created of any size, by using loops.

```cs
IEnumerable<int> GetRange(int from, int totalCount)
{
    for (int totalReturned = 0; totalReturned < totalCount; totalReturned++) 
    {
        yield return from + totalReturned;
    }
}

IEnumerable<int> valuesInRange = GetRange(6, 5);
foreach (int value in valuesInRange)
{
    Console.WriteLine(value);
}

// Output
// 6
// 7
// 8
// 9
// 10
```

# LINQ

So far, we've only seen how to use `IEnumerable` to generate data, but you can also use it to query data. This is where LINQ comes in. 

IEnumerable collections are not evaluated until `foreach` (or `IEnumerator.MoveNext()`) is called. This enables queries or operators to be stacked up together.

This is an example with a single query:

```cs
IEnumerable<int> allNumbers = GetRange(6, 5);
// Filter to only the even numbers from 6 to 10
IEnumerable<int> evenNumbers = allNumbers.Where(n => n % 2 == 0);

foreach (int value in evenNumbers)
{
    Console.WriteLine(value);
}

// Output
// 6
// 8
// 10
```

But queries can also be stacked, to work together
```cs
IEnumerable<int> allNumbers = GetRange(1, 20);

// Filter to only the numbers divisible by 2
IEnumerable<int> divisibleBy2 = allNumbers.Where(n => n % 2 == 0);

// Filter to only numbers divisible by 2 and 3
IEnumerable<int> divisibleBy2And3 = divisibleBy2.Where(n => n % 3 == 0);

foreach (int value in divisibleBy2And3)
{
    Console.WriteLine(value);
}

// Output
// 6
// 12
// 18
```

There are many operators (150+) besides `Where` built into .NET which allow you to transform data (`.Select`), group data (`.GroupBy`), sort data (`.OrderBy`), and more. You can find out more about those [here](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=netstandard-2.0#methods).

## LINQ that doesn't stack

Not all LINQ methods are created equal. Some can be stacked, such as `.Select`, `.GroupBy` and `.OrderBy`, but others cannot because they execute the query. You should be wary when you're using these, that you don't accidently execute an `IEnumerable` collection multiple times. 

Executing an `IEnumerable` collection multiple times is generally seen as a bad idea, because you don't know where it's getting the data from. If it's coming from a List, you're probably fine, but if it's coming from a complex source, such as a database, and you execute it more than once unecessarily, you're going to increase the load on the database, and slow down your application

For example:
```cs
IEnumerable<int> allNumbers = GetRange(6, 5);

// Count executes the IEnumerable in order to get the total number of items available
int totalItems = allNumbers.Count();
Console.WriteLine(totalItems);
// Output:
// 5

// This foreach then executes the IEnumerable a second time. You can verify this by putting a breakpoint inside GetRange.
foreach (int value in evenNumbers)
{
    Console.WriteLine(value);
}
// Output:
// 6
// 7
// 8
// 9
// 10
```

A better version would be:
```cs
IEnumerable<int> allNumbers = GetRange(6, 5);

// Count executes the IEnumerable in order to get the total number of items available
int totalItems = 0;

// This foreach then executes the IEnumerable a second time. You can verify this by putting a breakpoint inside GetRange.
foreach (int value in evenNumbers)
{
    totalItems++;
    Console.WriteLine(value);
}
// Output:
// 6
// 7
// 8
// 9
// 10

Console.WriteLine(totalItems);
// Output:
// 5
```

You can always tell which methods will execute an IEnumerable by looking at their return type. If they return an `IEnumerable`, then it can be stacked. If it returns a value (like `.Count` returns a number), then it probably executes the query. 

Examples include `.Count` (returns `int`), `.Any` (returns `bool`) and `.First` (returns `T`). There are many others too.

## Using IEnumerable in your own queries

Writing your own queries is not as complicated as it may seem. Thanks to `foreach` and the `yield` keyword you saw earlier, you can create your own query methods with relative ease.

```cs
IEnumerable<int> FilterEvenNumbers(IEnumerable<int> numbers)
{
    foreach (var number in numbers)
    {
        if (number % 2 == 0)
        {
            yield return number;
        }
    }
}

IEnumerable<int> allNumbers = GetRange(6, 10);
IEnumerable<int> evenNumbers = FilterEvenNumbers(allNumbers);

foreach (int number in evenNumbers)
{
    Console.WriteLine(value);
}

// Output
// 6
// 8
// 10
```

You can even stack your own queries with other .NET queries

```cs
IEnumerable<int> allNumbers = GetRange(6, 10);
IEnumerable<int> evenNumbers = FilterEvenNumbers(allNumbers);

// Take only the first 2 even numbers
evenNumbers = evenNumbers.Take(2);

foreach (int number in evenNumbers)
{
    Console.WriteLine(value);
}

// Output
// 6
// 8
```

# `List<T>` vs `IEnumerable<T>`/ `IEnumerator<T>`

Although these two things are often used interchangably (via `.ToList()` and `GetEnumerator`), and you can convert between the two, they are not the same. 

An `IEnumerable` collection can theoretically go on forever. A List<T> has a finite size.

For example:
```cs
IEnumerable<int> GetInfiniteZeros()
{
    while (true)
    {
        yield return 0;
    }
}

foreach(int value in GetInfiniteZeros())
{
    Console.WriteLine(value);
}

// Output:
// 0
// 0
// 0
// ...
```