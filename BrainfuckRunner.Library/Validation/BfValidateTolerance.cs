using System;

namespace BrainfuckRunner.Library.Validation
{
    /// <summary>
    /// Tolerance policy of validator
    /// </summary>
    [Flags]
    public enum BfValidateTolerance
    {
        /// <summary>
        /// No tolerance at all. Validator will consider new lines, whitespace characters
        /// and non-Brainfuck content as errors
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Validator is tolerant to non-Brainfuck content
        /// </summary>
        ToNonBrainfuckContent = 0x01,

        /// <summary>
        /// Validator is tolerant to whitespace characters
        /// </summary>
        ToWhiteSpaceContent = 0x02,

        /// <summary>
        /// Validator is tolerant to new lines
        /// </summary>
        ToNewLines = 0x04
    }
}
