using System;
using System.IO;
using BrainfuckRunner.Library.Behaviors;
using Microsoft.Extensions.Options;

namespace BrainfuckRunner.Library
{
    public sealed class BfEngineOptions : IOptions<BfEngineOptions>
    {
        public const int PresetTapeSize = 30_000;

        public int TapeSize { get; set; } = PresetTapeSize;

        public bool UseOptimizedExecutor { get; set; } = true;

        public BfCommentPattern CommentPattern { get; set; }

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

        public BfEngineOptions WithOptimizedExecutor()
        {
            UseOptimizedExecutor = true;
            return this;
        }

        public BfEngineOptions WithStandardExecutor()
        {
            UseOptimizedExecutor = false;
            return this;
        }

        public BfEngineOptions WithCommentPattern(BfCommentPattern pattern)
        {
            CommentPattern = pattern;
            return this;
        }

        public BfEngineOptions WithCommentPattern(string startTag, string endTag)
        {
            CommentPattern = new BfCommentPattern
            {
                StartTag = startTag,
                EndTag = endTag
            };
            return this;
        }

        BfEngineOptions IOptions<BfEngineOptions>.Value => this;
    }
}