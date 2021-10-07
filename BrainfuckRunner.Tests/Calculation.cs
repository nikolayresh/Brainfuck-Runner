using System.Collections.Generic;

namespace BrainfuckRunner.Tests
{
    internal static class Calculation
    {
        internal static uint MultiplyFactors(List<uint> factors)
        {
            uint result = (uint)1;
            factors.ForEach(x => result *= x);
            return result;
        }
    }
}