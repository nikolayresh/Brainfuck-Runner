using System;
using System.Collections.Generic;
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

        /// <summary>
        /// By default, a '#' character will be used to mark comment lines
        /// </summary>
        public const char PresetCommentToken = '#';

        internal HashSet<char> CommentTokens { get; private set; }

        internal int TapeSize { get; private set; } = PresetTapeSize;

        internal bool UseOptimizedExecutor { get; private set; } = true;

        internal TextReader Input { get; private set; } = Console.In;

        internal TextWriter Output { get; private set; } = Console.Out;

        internal BfMemoryOverflowBehavior OnMemoryOverflow { get; private set; } = BfMemoryOverflowBehavior.ThrowError;

        internal BfCellOverflowBehavior OnCellOverflow { get; private set; } = BfCellOverflowBehavior.ApplyOverflow;

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

        public BfEngineOptions WithCellOverflow(BfCellOverflowBehavior behavior)
        {
            OnCellOverflow = behavior;
            return this;
        }

        public BfEngineOptions WithMemoryOverflow(BfMemoryOverflowBehavior behavior)
        {
            OnMemoryOverflow = behavior;
            return this;
        }

        public BfEngineOptions WithDefaultCellOverflow()
        {
            OnCellOverflow = BfCellOverflowBehavior.ApplyOverflow;
            return this;
        }

        public BfEngineOptions WithDefaultMemoryOverflow()
        {
            OnMemoryOverflow = BfMemoryOverflowBehavior.ThrowError;
            return this;
        }

        public BfEngineOptions WithCommentToken(char token)
        {
            (CommentTokens ??= new HashSet<char>()).Add(token);
            return this;
        }

        public BfEngineOptions WithPresetCommentToken()
        {
            (CommentTokens ??= new HashSet<char>()).Clear();
            CommentTokens.Add(PresetCommentToken);
            return this;
        }

        BfEngineOptions IOptions<BfEngineOptions>.Value => this;
    }
}