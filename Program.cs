using System;
using System.Collections.Generic;
using static System.Console;

namespace Explore_CSharp7
{
  class Program
  {
    static void Main(string[] args)
    {
      var input = "1234";
      int numericResult;
      if (int.TryParse(input, out numericResult))
          WriteLine(numericResult);
      else
          WriteLine("Could not parse input");

      // Can now declare out variables in the argument list of a method call,
      // rather than writing a separate declaration statement.
      // 
      if (int.TryParse(input, out var result))
      {
        WriteLine(result);
      }
      else
      {
        WriteLine("Could not parse input");
      }

      /** 
       * Tuples are lightweight data structures that contain multiple fields to
       * represent the data members
      */
      (string Alpha, string Beta) namedLetters = ("a", "b");
      WriteLine($"{namedLetters.Alpha}, {namedLetters.Beta}");

      // Can also specify the names of the fields on the right-hand side of the
      // assignment, but the names on the right side are ignored.
      var alphabetStart = (Alpha: "a", Beta: "b");
      WriteLine($"{alphabetStart.Alpha}, {alphabetStart.Beta}");

      /** 
       * Tuples are most useful as return types for private and internal methods.
       * Tuples provide a simple syntax for those methods to return multiple discrete values.
       * Creating a tuple is more efficient and more productive than creating a
       * class or struct. It has a simpler, lightweight syntax to define a data
       * structure that carries more than one value.
      */
      (int Max, int Min) Range(IEnumerable<int> sequence)
      {
        int Min = int.MaxValue;
        int Max = int.MinValue;
        foreach (var n in sequence)
        {
          /** 
           * https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
           * The conditional (ternary conditional) operator ?:
           * condition ? consequent : alternative
           *                   true : false        
          */
          //    condition ? consequent (true) : alternative (false)
          Min = (n < Min) ? n : Min;
          Max = (n > Max) ? n : Max;
        }
        return (Max, Min);
      }

      var numbers = new int[] { 1, 2, 3, 5, 8, 13, 21, 34, 55 };
      var range = Range(numbers);
      WriteLine(range);

      // To unpackage the members of a tuple that were returned from a method, you
      // declare separate variables for each of the values in the tuple. Called
      // deconstructing the tuple.
      (int max, int min) = Range(numbers);
      WriteLine(max);
      WriteLine(min);

      // ...don't use all of the members of a tuple result. You can discard one
      // or more of the returned values by using _ in place of a variable.
      (int maxValue, _) = Range(numbers);
      WriteLine(maxValue);

      // Use the type pattern with the is expression
      object count = 5;

      if (count is int number)
      {
        // number is only assigned in the true branch.
        WriteLine(number);
      }
      else
      {
        WriteLine($"{count} is not an integer");
      }

      /** 
       * Minimize access to code with local functions
       * Common uses are for public iterator methods and public async methods.
       * Both types of methods generate code that reports errors later than programmers
       * might expect. In the case of iterator methods, any exceptions are observed
       * only when calling code that enumerates that returned sequence. In the
       * case of async methods, any exceptions are only observed when the returned
       * Task is awaited.
       * 
       * Run the code. An exception is thrown when the code begins iterating the
       * second result set. The code that iterates the first result set has already
       * run...If the first iterator method changed data state, it could cause
       * data corruption. Better if the exception is thrown immediately, before
       * any work is done.
       * Refactor the code so that the public method validates all arguments,
       * and a local function performs the enumeration.
       * JOSE: With the refactored code, the exception is thrown before any work
       * is done.
      */
      IEnumerable<char> AlphabetSubset(char start, char end)
      {
        if (start < 'a' || start > 'z')
        {
          throw new ArgumentOutOfRangeException(paramName: nameof(start),
            message: "start must be a letter");
        }
        if (end < 'a' || end > 'z')
        {
          throw new ArgumentOutOfRangeException(paramName: nameof(end),
            message: "end must be a letter");
        }
        if (end <= start)
        {
          throw new ArgumentException($"{nameof(end)} must be greater than {nameof(start)}");
        }

        /** 
         * This version makes it clear that the local method is referenced only
         * in the context of the outer method. The rules for local functions also
         * ensure that a developer cannot accidentally call the local function from
         * another location in the class and bypass the argument validation.
         * 
         * Same technique can be employed with async methods to ensure that exceptions
         * arising from argument validation are thrown before the asynchronous work begins.       
        */
        return alphabetSubsetImplementation();

        IEnumerable<char> alphabetSubsetImplementation()
        {
          for (var c = start; c < end; c++)
          {
            yield return c;
          }
        }

        /*
        for (var c = start; c < end; c++)
        {
          yield return c;
        }
        */
      }

      try
      {
        var resultSet1 = AlphabetSubset('d', 'r');
        var resultSet2 = AlphabetSubset('f', 'a');
        WriteLine("iterators created");
      
        foreach (var thing1 in resultSet1)
          Write($"{thing1}, ");
        WriteLine();

        foreach (var thing2 in resultSet2)
          Write($"{ thing2}, ");
        WriteLine();
      }
      catch (ArgumentException)
      {
        WriteLine("Caught an argument exception");
      }
    }
  }
}
