using System.Text;
using BrainfuckRunner.Library;
using BrainfuckRunner.Library.Behaviors;
using Xunit;

namespace BrainfuckRunner.Tests
{
    public class CellOverflowTests
    {
        [Theory(DisplayName = "Calculation of modulo with respect to base threshold")]
        [InlineData(300, 256, 44)]
        [InlineData(-1, 256, 255)]
        [InlineData(-2, 256, 254)]
        [InlineData(-257, 256, 255)]
        [InlineData(-300, 256, 212)]
        [InlineData(256, 256, 0)]
        [InlineData(0, 256, 0)]
        [InlineData(-712, 256, 56)]
        [InlineData(712, 256, 200)]
        [InlineData(-2, 30000, 29998)]
        [InlineData(0, 30000, 0)]
        [InlineData(1, 30000, 1)]
        [InlineData(29999,30000, 29999)]
        public void theory_BfExtensions_Mod(int value, int @base, int expected)
        {
            var result = BfExtensions.Mod(value, @base);
            Assert.Equal(expected, result);
        }

        [Theory(DisplayName = "Cell overflows when behavior [ApplyOverflow] set")]
        [InlineData(5, 251)]
        [InlineData(1, 255)]
        [InlineData(2, 254)]
        public void cell_overflows_when_behavior_ApplyOverflow_set(int delta, byte expected)
        {
            var engine = new BfEngine(
                BfMemoryOverflowBehavior.ThrowError,
                BfCellOverflowBehavior.ApplyOverflow);

            var script = new StringBuilder();
            while (delta-- != 0)
            {
                script.Append('-');
            }

            engine.ExecuteScript(script.ToString());

            Assert.Equal(expected, engine.Cells[0]);
        }
    }
}
