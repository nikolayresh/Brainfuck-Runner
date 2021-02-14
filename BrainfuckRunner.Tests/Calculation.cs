using System;
using System.Collections.Generic;

namespace BrainfuckRunner.Tests
{
    internal static class Calculation
    {
        internal static uint MultiplyFactors(List<uint> factors)
        {
            var result = (uint)1;
            factors.ForEach(x => result *= x);
            return result;
        }

        internal static IEnumerable<uint> EnumeratePrimesUpTo(uint threshold)
        {
            if (threshold < 2)
            {
                yield break;
            }

            yield return 2; // number 2 - is the first prime number

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
            for (uint divisor = 2; divisor <= (uint) Math.Floor(Math.Sqrt(number)); divisor++)
            {
                if (number % divisor == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}