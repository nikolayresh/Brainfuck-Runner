using System;
using System.IO;
using BrainfuckRunner.Library.Behaviors;
using Microsoft.Extensions.Options;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Options used to adjust execution of Brainfuck interpreting engine
    /// </summary>
    public sealed class BfEngineOptions : IOptions<BfEngineOptions>
    {
        /// <summary>
        /// By default, Brainfuck interpreter uses 30,000 cells for most cases
        /// </summary>
        public const int PresetTapeSize = 30_000;

        public int TapeSize { get; set; } = PresetTapeSize;

        public bool UseOptimizedExecutor { get; set; } = true;

        public TextReader Input { get; set; } = Console.In;

        public TextWriter Output { get; set; } = Console.Out;

        public BfMemoryOverflowBehavior OnMemoryOverflow { get; set; } = BfMemoryOverflowBehavior.ThrowError;

        public BfCellOverflowBehavior OnCellOverflow { get; set; } = BfCellOverflowBehavior.ApplyOverflow;

        public BfEngineOptions WithTapeSize(int size)
        {
            TapeSize = size;
            return this;
        }

        public BfEngineOptions WithPresetTapeSize()
        {
            TapeSize = PresetTapeSize;
            return this;
        }

        public BfEngineOptions WithInput(TextReader input)
        {
            Input = input;
            return this;
        }

        public BfEngineOptions WithOutput(TextWriter output)
        {
            Output = output;
            return this;
        }

        public BfEngineOptions WithConsoleInput()
        {
            Input = Console.In;
            return this;
        }

        public BfEngineOptions WithConsoleOutput()
        {
            Output = Console.Out;
            return this;
        }

        public BfEngineOptions WithOptimizedExecutor()
        {
            UseOptimizedExecutor = true;
            return this;
        }

        public BfEngineOptions WithSimpleExecutor()
        {
            UseOptimizedExecutor = false;
            return this;
        }

        BfEngineOptions IOptions<BfEngineOptions>.Value => this;
    }
}