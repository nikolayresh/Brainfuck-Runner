using System.IO;
using System.Linq;
using BrainfuckRunner.Library;
using Xunit;
using Xunit.Abstractions;

namespace BrainfuckRunner.Tests
{
    public class FileExecutorTests
    {
        private readonly ITestOutputHelper _output;

        public FileExecutorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "Letter A")]
        [Trait("Is optimized", "No")]
        public void execute_LetterA()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("LetterA.bf"));
            var output = sw.ToString();

            Assert.Equal("A", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Letter A (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_LetterA()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("LetterA.bf"));
            var output = sw.ToString();

            Assert.Equal("A", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Mandel")]
        [Trait("Is optimized", "No")]
        public void execute_Mandel()
        {
            var engine = new BfEngine(new BfEngineOptions()
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("Mandel.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Mandel (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Mandel()
        {
            var engine = new BfEngine(new BfEngineOptions()
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("Mandel.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hello World!")]
        [Trait("Is optimized", "No")]
        public void execute_HelloWorld()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("HelloWorld.bf"));
            var output = sw.ToString();

            Assert.Equal("Hello World!\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hello World! (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_HelloWorld()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("HelloWorld.bf"));
            var output = sw.ToString();

            Assert.Equal("Hello World!\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Busy Beaver")]
        [Trait("Is optimized", "No")]
        public void execute_BusyBeaver()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("BusyBeaver.bf"));
            var output = sw.ToString();

            Assert.Equal("OK\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Busy Beaver (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_BusyBeaver()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("BusyBeaver.bf"));
            var output = sw.ToString();

            Assert.Equal("OK\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 1")]
        [Trait("Is optimized", "No")]
        public void execute_Bench1()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("BenchOne.bf"));
            var output = sw.ToString();

            Assert.Equal("OK", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 1 (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Bench1()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("BenchOne.bf"));
            var output = sw.ToString();

            Assert.Equal("OK", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hanoi")]
        [Trait("Is optimized", "No")]
        public void execute_Hanoi()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("Hanoi.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Hanoi (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Hanoi()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("Hanoi.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 2")]
        [Trait("Is optimized", "No")]
        public void execute_Bench2()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var expected = string.Empty;
            var ch = 'Z';

            do
            {
                expected += ch;
            } while (ch-- != 'A');

            var elapsed = engine.Execute(OpenText("BenchTwo.bf"));
            var output = sw.ToString();

            Assert.Equal($"{expected}\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Bench 2 (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Bench2()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var expected = string.Empty;
            var ch = 'Z';

            do
            {
                expected += ch;
            } while (ch-- != 'A');

            var elapsed = engine.Execute(OpenText("BenchTwo.bf"));
            var output = sw.ToString();
         
            Assert.Equal($"{expected}\n", output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Long Running")]
        [Trait("Is optimized", "No")]
        public void execute_LongRunningFile()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var expected = ((char)202).ToString();
            var elapsed = engine.Execute(OpenText("Long.bf"));

            Assert.Equal(expected, sw.ToString());
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Long Running (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_LongRunningFile()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var expected = ((char)202).ToString();
            var elapsed = engine.Execute(OpenText("Long.bf"));

            Assert.Equal(expected, sw.ToString());
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Display ASCII")]
        [Trait("Is optimized", "No")]
        public void execute_DisplayAscii()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("DisplayAscii.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Display ASCII (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_DisplayAscii()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("DisplayAscii.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Alphabet")]
        [Trait("Is optimized", "No")]
        public void execute_Alphabet()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithSimpleExecutor());

            var expected = string.Empty;
            var ch = 'A';

            do
            {
                expected += ch;
            } while (ch++ != 'Z');

            var elapsed = engine.Execute(OpenText("Alphabet.bf"));
            var output = sw.ToString();

            Assert.Equal(expected, output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "Alphabet (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_Alphabet()
        {
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var expected = string.Empty;
            var ch = 'A';

            do
            {
                expected += ch;
            } while (ch++ != 'Z');

            var elapsed = engine.Execute(OpenText("Alphabet.bf"));
            var output = sw.ToString();

            Assert.Equal(expected, output);
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "99 Bottles of Beer")]
        [Trait("Is optimized", "No")]
        public void execute_99_Bottles_Of_Beer()
        {
            var engine = new BfEngine(new BfEngineOptions()
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("99_Bottles_Of_Beer.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Fact(DisplayName = "99 Bottles of Beer (optimized)")]
        [Trait("Is optimized", "Yes")]
        public void execute_optimized_99_Bottles_Of_Beer()
        {
            var engine = new BfEngine(new BfEngineOptions()
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("99_Bottles_Of_Beer.bf"));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "Calculate factors of a number")]
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
            var sr = new StringReader($"{number}\n");
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("Factor.bf"));
            var resultLine = sw.ToString();
            var parts = resultLine.Split(':');

            Assert.True(parts.Length == 2);
            Assert.True(uint.TryParse(parts[0], out _));
            Assert.Equal(number, uint.Parse(parts[0]));

            var factors = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            Assert.Equal(number, Calculation.MultiplyFactors(factors));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "Calculate multipliers/factors of a number (optimized)")]
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
            var sr = new StringReader($"{number}\n");
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("Factor.bf"));
            var resultLine = sw.ToString();
            var parts = resultLine.Split(':');

            Assert.True(parts.Length == 2);
            Assert.True(uint.TryParse(parts[0], out _));
            Assert.Equal(number, uint.Parse(parts[0]));

            var factors = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            Assert.Equal(number, Calculation.MultiplyFactors(factors));
            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "List of prime numbers up to specified threshold")]
        [Trait("Is optimized", "No")]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        [InlineData(25)]
        [InlineData(30)]
        [InlineData(35)]
        [InlineData(40)]
        [InlineData(45)]
        [InlineData(50)]
        [InlineData(55)]
        [InlineData(60)]
        [InlineData(65)]
        [InlineData(70)]
        [InlineData(75)]
        [InlineData(80)]
        [InlineData(85)]
        [InlineData(90)]
        [InlineData(95)]
        [InlineData(100)]
        [InlineData(105)]
        [InlineData(110)]
        [InlineData(115)]
        [InlineData(120)]
        [InlineData(125)]
        [InlineData(130)]
        [InlineData(135)]
        [InlineData(140)]
        [InlineData(145)]
        [InlineData(150)]
        [InlineData(155)]
        [InlineData(160)]
        [InlineData(165)]
        [InlineData(170)]
        [InlineData(175)]
        [InlineData(180)]
        [InlineData(185)]
        [InlineData(190)]
        [InlineData(195)]
        [InlineData(200)]
        [InlineData(205)]
        [InlineData(210)]
        [InlineData(215)]
        [InlineData(220)]
        [InlineData(225)]
        [InlineData(230)]
        [InlineData(235)]
        [InlineData(240)]
        [InlineData(245)]
        [InlineData(250)]
        [InlineData(255)]
        [InlineData(256)]
        public void execute_Prime_Numbers(uint threshold)
        {
            var sr = new StringReader(threshold.ToString());
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithSimpleExecutor());

            var elapsed = engine.Execute(OpenText("PrimeNumbers.bf"));
            var output = sw.ToString();
            var parts = output.Split(':');

            Assert.True(parts.Length == 2);
            Assert.Equal("Primes up to", parts[0]);

            var primes = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            var expectedPrimes = Calculation.EnumeratePrimesUpTo(threshold).ToList();
            Assert.Equal(expectedPrimes, primes);

            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        [Theory(DisplayName = "List of prime numbers up to specified threshold (optimized)")]
        [Trait("Is optimized", "Yes")]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        [InlineData(25)]
        [InlineData(30)]
        [InlineData(35)]
        [InlineData(40)]
        [InlineData(45)]
        [InlineData(50)]
        [InlineData(55)]
        [InlineData(60)]
        [InlineData(65)]
        [InlineData(70)]
        [InlineData(75)]
        [InlineData(80)]
        [InlineData(85)]
        [InlineData(90)]
        [InlineData(95)]
        [InlineData(100)]
        [InlineData(105)]
        [InlineData(110)]
        [InlineData(115)]
        [InlineData(120)]
        [InlineData(125)]
        [InlineData(130)]
        [InlineData(135)]
        [InlineData(140)]
        [InlineData(145)]
        [InlineData(150)]
        [InlineData(155)]
        [InlineData(160)]
        [InlineData(165)]
        [InlineData(170)]
        [InlineData(175)]
        [InlineData(180)]
        [InlineData(185)]
        [InlineData(190)]
        [InlineData(195)]
        [InlineData(200)]
        [InlineData(205)]
        [InlineData(210)]
        [InlineData(215)]
        [InlineData(220)]
        [InlineData(225)]
        [InlineData(230)]
        [InlineData(235)]
        [InlineData(240)]
        [InlineData(245)]
        [InlineData(250)]
        [InlineData(255)]
        [InlineData(256)]
        public void execute_optimized_Prime_Numbers(uint threshold)
        {
            var sr = new StringReader(threshold.ToString());
            var sw = new StringWriter();
            var engine = new BfEngine(new BfEngineOptions()
                .WithInput(sr)
                .WithOutput(sw)
                .WithOptimizedExecutor());

            var elapsed = engine.Execute(OpenText("PrimeNumbers.bf"));
            var output = sw.ToString();
            var parts = output.Split(':');

            Assert.True(parts.Length == 2);
            Assert.Equal("Primes up to", parts[0]);

            var primes = parts[1].Trim()
                .Split((char)32)
                .Select(uint.Parse)
                .ToList();

            var expectedPrimes = Calculation.EnumeratePrimesUpTo(threshold).ToList();
            Assert.Equal(expectedPrimes, primes);

            _output.WriteLine($"Time taken to execute: {elapsed:c}");
        }

        private static TextReader OpenText(string file)
        {
            var path = string.Concat("BrainfuckRunner.Tests.CodeFiles.", file);
            var stream = typeof(FileExecutorTests).Assembly.GetManifestResourceStream(path);
            return new StreamReader(stream);
        }
    }
}