using System.Text;
using BrainfuckRunner.Library;
using BrainfuckRunner.Library.Behaviors;
using Xunit;

namespace BrainfuckRunner.Tests
{
    public class PointerOverflowTests
    {
        [Theory(DisplayName = "Pointer overflows to end of tape")]
        [InlineData(0,  1, 29999)]
        [InlineData(0,  2, 29998)]
        [InlineData(1,  2, 29999)]
        [InlineData(2,  3, 29999)]
        [InlineData(3, 10, 29993)]
        [InlineData(4,  5, 29999)]
        [InlineData(4,  6, 29998)]
        public void pointer_overflows_to_end_of_tape_fixed_pos(int initPos, int shifts, int expectedPtr)
        {
            var engine = new BfEngine(
                BfMemoryOverflowBehavior.ApplyOverflow,
                BfCellOverflowBehavior.ApplyOverflow);

            var script = new StringBuilder();

            while (initPos-- > 0)
            {
                script.Append('>');
            }

            while (shifts-- > 0)
            {
                script.Append('<');
            }

            engine.ExecuteScript(script.ToString());

            Assert.Equal(expectedPtr, engine.Pointer);
        }

        [Theory(DisplayName = "Pointer overflows to end of tape")]
        [InlineData(0, 0)]
        [InlineData(1, 29999)]
        [InlineData(2, 29998)]
        [InlineData(3, 29997)]
        [InlineData(4, 29996)]
        [InlineData(5, 29995)]

        [InlineData(30000, 0)]
        [InlineData(30001, 29999)]
        [InlineData(30002, 29998)]
        [InlineData(30003, 29997)]
        [InlineData(30004, 29996)]
        [InlineData(30005, 29995)]

        [InlineData(60000, 0)]
        [InlineData(60001, 29999)]
        [InlineData(60002, 29998)]
        [InlineData(60003, 29997)]
        [InlineData(60004, 29996)]
        [InlineData(60005, 29995)]

        [InlineData(90000, 0)]
        [InlineData(90001, 29999)]
        [InlineData(90002, 29998)]
        [InlineData(90003, 29997)]
        [InlineData(90004, 29996)]
        [InlineData(90005, 29995)]
        public void pointer_overflows_to_end_of_tape(int shifts, int expectedPtr)
        {
           var engine = new BfEngine(
               BfMemoryOverflowBehavior.ApplyOverflow,
               BfCellOverflowBehavior.ApplyOverflow);
           var script = new StringBuilder();

           while (shifts-- > 0)
           {
               script.Append('<');
           }

           engine.ExecuteScript(script.ToString());

           Assert.Equal(expectedPtr, engine.Pointer);
        }

        [Theory(DisplayName = "Pointer overflows to start of tape")]
        [InlineData(0, 29999)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 3)]
        [InlineData(5, 4)]

        [InlineData(30000, 29999)]
        [InlineData(30001, 0)]
        [InlineData(30002, 1)]
        [InlineData(30003, 2)]
        [InlineData(30004, 3)]
        [InlineData(30005, 4)]

        [InlineData(60000, 29999)]
        [InlineData(60001, 0)]
        [InlineData(60002, 1)]
        [InlineData(60003, 2)]
        [InlineData(60004, 3)]
        [InlineData(60005, 4)]

        [InlineData(90000, 29999)]
        [InlineData(90001, 0)]
        [InlineData(90002, 1)]
        [InlineData(90003, 2)]
        [InlineData(90004, 3)]
        [InlineData(90005, 4)]
        public void pointer_overflows_to_start(int shifts, int expectedPtr)
        {
            var engine = new BfEngine(
                BfMemoryOverflowBehavior.ApplyOverflow,
                BfCellOverflowBehavior.ApplyOverflow);
            var script = new StringBuilder();

            for (int i = 0; i < engine.TapeSize - 1; ++i)
            {
                script.Append('>');
            }

            while (shifts-- > 0)
            {
                script.Append('>');
            }

            engine.ExecuteScript(script.ToString());

            Assert.Equal(expectedPtr, engine.Pointer);
        }

        [Fact]
        public void should_throw_when_behavior_ThrowError_set_pointer_at_start()
        {
            var engine = new BfEngine(
                BfMemoryOverflowBehavior.ThrowError,
                BfCellOverflowBehavior.ApplyOverflow);

            const string script = "<";

            Assert.Throws<BfException>(() => engine.ExecuteScript(script));
        }

        [Fact]
        public void should_throw_when_behavior_ThrowError_set_pointer_at_the_end()
        {
            var engine = new BfEngine(
                BfMemoryOverflowBehavior.ThrowError,
                BfCellOverflowBehavior.ApplyOverflow);

            var script = new StringBuilder();

            for (int i = 0; i < engine.TapeSize - 1; ++i)
            {
                script.Append('>');
            }

            script.Append('>');

            Assert.Throws<BfException>(
                () => engine.ExecuteScript(script.ToString()));
        }
    }
}
