using System;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Exception thrown by Brainfuck engine while execution
    /// </summary>
    public sealed class BfException : Exception
    {
        public BfException(BfRuntimeError code, string msg) : base(msg)
        {
            ErrorCode = code;
        }

        /// <summary>
        /// Code of error
        /// </summary>
        public BfRuntimeError ErrorCode
        {
            get;
        }

        public override string Source => "Brainfuck Engine";
    }
}