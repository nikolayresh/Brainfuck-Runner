using System;
using System.Collections.Generic;

namespace BrainfuckRunner.Tests
{
    internal static class Calculation
    {
        internal static uint MultiplyFactors(List<uint> factors)
        {
            var result = (uint)1;

            while (factors.Count != 0)
            {
                result *= factors[0];
                factors.RemoveAt(0);
            }

            return result;
        }

        internal static IEnumerable<uint> EnumeratePrimesUpTo(uint threshold)
        {
            if (threshold < 2)
            {
                yield break;
            }

            yield return (uint)2; // number 2 - is the first prime number

            for (uint number = 3; number <= threshold; number += 2)
            {
                if (IsPrimeNumber(number))
                {
                    yield return number;
                }
            }
        }

        private static bool IsPrimeNumber(uint number)
        {
            for (uint divider = 2; divider <= (uint) Math.Sqrt(number); divider++)
            {
                if ((number % divider) == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}