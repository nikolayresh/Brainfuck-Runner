using System;
using System.IO;
using BrainfuckRunner.Library;
using Xunit;

namespace BrainfuckRunner.Tests
{
    public class FileExecutorTimeoutTests
    {
        [Fact]
        public void execute_LongRunningFile_with_timeout_should_throw_TimeoutException()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            StringWriter sw = new StringWriter();
            BfEngine engine = new BfEngine(new BfEngineOptions()
                .WithOutput(sw)
                .WithOptimizedExecutor());

            Assert.Throws<TimeoutException>(() => engine.Execute(OpenText("Long.bf"), timeout));
        }

        private static TextReader OpenText(string file)
        {
            string resourcePath = string.Join(".", "BrainfuckRunner.Tests.CodeFiles", file);
            Stream resource = typeof(FileExecutorTests).Assembly.GetManifestResourceStream(resourcePath);

            return resource != null ? new StreamReader(resource) : null;
        }
    }
}