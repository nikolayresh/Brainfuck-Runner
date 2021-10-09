using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrainfuckRunner.Library;
using Xunit;
using Xunit.Abstractions;

namespace BrainfuckRunner.Tests
{
    public class FileExecutorTests
    {
        private const int RunTimeout = 5 * 60 * 1000;
        private readonly ITestOutputHelper _output;

        public FileExecutorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "Letter A", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_LetterA()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("LetterA.bf"));
            string output = sw.ToString();

            Assert.Equal("A", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Letter A (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_LetterA()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("LetterA.bf"));
            string output = sw.ToString();

            Assert.Equal("A", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Mandel", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Mandel()
        {
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("Mandel.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Mandel (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Mandel()
        {
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("Mandel.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hello World!", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_HelloWorld()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("HelloWorld.bf"));
            string output = sw.ToString();

            Assert.Equal("Hello World!\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hello World! (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_HelloWorld()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("HelloWorld.bf"));
            string output = sw.ToString();

            Assert.Equal("Hello World!\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Busy Beaver", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_BusyBeaver()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("BusyBeaver.bf"));
            string output = sw.ToString();

            Assert.Equal("OK\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Busy Beaver (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_BusyBeaver()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("BusyBeaver.bf"));
            string output = sw.ToString();

            Assert.Equal("OK\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 1", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Bench1()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("BenchOne.bf"));
            string output = sw.ToString();

            Assert.Equal("OK", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 1 (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Bench1()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("BenchOne.bf"));
            string output = sw.ToString();

            Assert.Equal("OK", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hanoi", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Hanoi()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("Hanoi.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hanoi (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Hanoi()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("Hanoi.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 2", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Bench2()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            string expected = string.Empty;
            char ch = 'Z';

            do
            {
                expected += ch;
            } while (ch-- != 'A');

            TimeSpan elapsed = engine.Execute(OpenText("BenchTwo.bf"));
            string output = sw.ToString();

            Assert.Equal($"{expected}\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 2 (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Bench2()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            string expected = string.Empty;
            char ch = 'Z';

            do
            {
                expected += ch;
            } while (ch-- != 'A');

            TimeSpan elapsed = engine.Execute(OpenText("BenchTwo.bf"));
            string output = sw.ToString();
         
            Assert.Equal($"{expected}\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Long Running", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_LongRunningFile()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            string expected = ((char)202).ToString();
            TimeSpan elapsed = engine.Execute(OpenText("Long.bf"));

            Assert.Equal(expected, sw.ToString());
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Long Running (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_LongRunningFile()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            string expected = ((char)202).ToString();
            TimeSpan elapsed = engine.Execute(OpenText("Long.bf"));

            Assert.Equal(expected, sw.ToString());
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Display ASCII", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_DisplayAscii()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("DisplayAscii.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Display ASCII (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_DisplayAscii()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            TimeSpan elapsed = engine.Execute(OpenText("DisplayAscii.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Alphabet", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Alphabet()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            string expected = string.Empty;
            char ch = 'A';

            do
            {
                expected += ch;
            } while (ch++ != 'Z');

            TimeSpan elapsed = engine.Execute(OpenText("Alphabet.bf"));
            string output = sw.ToString();

            Assert.Equal(expected, output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Alphabet (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Alphabet()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            string expected = string.Empty;
            char ch = 'A';

            do
            {
                expected += ch;
            } while (ch++ != 'Z');

            TimeSpan elapsed = engine.Execute(OpenText("Alphabet.bf"));
            string output = sw.ToString();

            Assert.Equal(expected, output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "99 Bottles of Beer", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_99_Bottles_Of_Beer()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithSimpleExecutor()
                .WithOutput(sw)
                .WithPresetCommentToken());

            TimeSpan elapsed = engine.Execute(OpenText("99_Bottles_Of_Beer.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "99 Bottles of Beer (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_99_Bottles_Of_Beer()
        {
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOptimizedExecutor()
                .WithPresetCommentToken());

            TimeSpan elapsed = engine.Execute(OpenText("99_Bottles_Of_Beer.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Quine", Timeout = RunTimeout)]
        [Trait("Is optimized", "No")]
        public void execute_Quine()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor()
                .WithPresetCommentToken());

            TimeSpan elapsed = engine.Execute(OpenText("quine.bf"));
            string output = sw.ToString();

            Assert.Equal("->++>+++>+>+>+++>>>>>>>>>>>>>>>>>>>>>>+>+>++>+++>++>>+++>+>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>+>+>>+++>>>>+++>>>+++>+>>>>>>>++>+++>+++>+>>+++>+++>+>+++>+>+++>+>++>+++>>>+>+>+>+>++>+++>+>+>>+++>>>>>>>+>+>>>+>+>++>+++>+++>+>>+++>+++>+>+++>+>++>+++>++>>+>+>++>+++>+>+>>+++>>>+++>+>>>++>+++>+++>+>>+++>>>+++>+>+++>+>>+++>>+++>>+[[>>+[>]+>+[<]<-]>>[>]<+<+++[<]<<+]>>>[>]+++>+[+[<++++++++++++++++>-]<++++++++++.<]", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Quine (optimized)", Timeout = RunTimeout)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Quine()
        {
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor()
                .WithPresetCommentToken());

            TimeSpan elapsed = engine.Execute(OpenText("quine.bf"));
            string output = sw.ToString();

            Assert.Equal("->++>+++>+>+>+++>>>>>>>>>>>>>>>>>>>>>>+>+>++>+++>++>>+++>+>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>+>+>>+++>>>>+++>>>+++>+>>>>>>>++>+++>+++>+>>+++>+++>+>+++>+>+++>+>++>+++>>>+>+>+>+>++>+++>+>+>>+++>>>>>>>+>+>>>+>+>++>+++>+++>+>>+++>+++>+>+++>+>++>+++>++>>+>+>++>+++>+>+>>+++>>>+++>+>>>++>+++>+++>+>>+++>>>+++>+>+++>+>>+++>>+++>>+[[>>+[>]+>+[<]<-]>>[>]<+<+++[<]<<+]>>>[>]+++>+[+[<++++++++++++++++>-]<++++++++++.<]", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "Calculate multipliers/factors of a number", Timeout = RunTimeout)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(18)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(24)]
        [InlineData(25)]
        [InlineData(30)]
        [InlineData(31)]
        [InlineData(37)]
        [InlineData(40)]
        [InlineData(50)]
        [InlineData(75)]
        [InlineData(100)]
        [Trait("Is optimized", "No")]
        public void execute_Factor(uint number)
        {
            StringReader sr = new StringReader($"{number}\n");
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithSimpleExecutor()
                .WithCommentToken('*'));

            TimeSpan elapsed = engine.Execute(OpenText("Factor.bf"));
            string resultLine = sw.ToString();
            string[] parts = resultLine.Split(':');

            Assert.True(parts.Length == 2);
            Assert.True(uint.TryParse(parts[0], out _));
            Assert.Equal(number, uint.Parse(parts[0]));

            List<uint> factors = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            Assert.Equal(number, Calculation.MultiplyFactors(factors));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "Calculate multipliers/factors of a number (optimized)", Timeout = RunTimeout)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(18)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(22)]
        [InlineData(24)]
        [InlineData(25)]
        [InlineData(30)]
        [InlineData(31)]
        [InlineData(37)]
        [InlineData(40)]
        [InlineData(50)]
        [InlineData(75)]
        [InlineData(100)]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Factor(uint number)
        {
            StringReader sr = new StringReader($"{number}\n");
            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithOptimizedExecutor()
                .WithCommentToken('*'));

            TimeSpan elapsed = engine.Execute(OpenText("Factor.bf"));
            string resultLine = sw.ToString();
            string[] parts = resultLine.Split(':');

            Assert.True(parts.Length == 2);
            Assert.True(uint.TryParse(parts[0], out _));
            Assert.Equal(number, uint.Parse(parts[0]));

            List<uint> factors = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            Assert.Equal(number, Calculation.MultiplyFactors(factors));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        private static TextReader OpenText(string file)
        {
            string path = Path.Combine("CodeFiles", file);
            return File.OpenText(path);
        }
    }
}