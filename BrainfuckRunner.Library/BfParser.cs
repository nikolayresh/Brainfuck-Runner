using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BrainfuckRunner.Library
{
    internal sealed class BfParser
    {
        /// <summary>
        /// Defines state when end of file reached
        /// </summary>
        internal const int Eof = -1;

        internal const int LineFeed = 10;
        internal const int CarriageReturn = 13;
        internal const int WhiteSpace = 32;

        /// <summary>
        /// All Brainfuck commands placed into set
        /// </summary>
        internal static readonly string CommandSet;

        internal const char MoveBackwardCmd = '<';
        internal const char MoveForwardCmd = '>';
        internal const char DecrementCmd = '-';
        internal const char IncrementCmd = '+';
        internal const char ReadCmd = ',';
        internal const char PrintCmd = '.';
        internal const char OpenLoopCmd = '[';
        internal const char CloseLoopCmd = ']';

        static BfParser()
        {
            CommandSet = new StringBuilder()
                .Append(MoveBackwardCmd)
                .Append(MoveForwardCmd)
                .Append(DecrementCmd)
                .Append(IncrementCmd)
                .Append(ReadCmd)
                .Append(PrintCmd)
                .Append(OpenLoopCmd)
                .Append(CloseLoopCmd)
                .ToString();
        }

        private readonly TextReader _text;
        private readonly char[] _commentTokens;

        internal BfParser(TextReader text, char[] commentTokens)
        {
            _text = text;
            _commentTokens = commentTokens;
        }

        /// <summary>
        /// Returns a boolean value whether specified character
        /// is a valid Brainfuck command
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsBrainfuckCommand(int arg, out BfCommand cmd)
        {
            cmd = BfCommand.Unknown;
            char ch = (char) arg;

            cmd = ch switch
            {
                MoveBackwardCmd => BfCommand.MoveBackward,
                MoveForwardCmd => BfCommand.MoveForward,
                DecrementCmd => BfCommand.Decrement,
                IncrementCmd => BfCommand.Increment,
                ReadCmd => BfCommand.Read,
                PrintCmd => BfCommand.Print,
                OpenLoopCmd => BfCommand.OpenLoop,
                CloseLoopCmd => BfCommand.CloseLoop,
                _ => cmd
            };

            return cmd != BfCommand.Unknown;
        }

        private void SkipWhiteSpace()
        {
            while (_text.Peek() == WhiteSpace)
            {
                _text.Read();
            }
        }

        private bool TryForNewLine()
        {
            int @char = _text.Read();

            if (@char is CarriageReturn or LineFeed)
            {
                if (OperatingSystem.IsWindows() && @char is CarriageReturn && _text.Peek() is LineFeed)
                {
                    _text.Read();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a next parsed Brainfuck command
        /// </summary>
        internal BfCommand ParseNextCommand()
        {
            bool withinComment = false;

            while (_text.Peek() != Eof)
            {
                SkipWhiteSpace();

                if (!withinComment)
                {
                    if (IsBrainfuckCommand(_text.Peek(), out BfCommand cmd))
                    {
                        _text.Read();
                        return cmd;
                    }

                    withinComment = _commentTokens != null && _commentTokens.Any(ch => ch == (char) _text.Peek());
                    _text.Read();
                }
                else
                {
                    if (TryForNewLine()) withinComment = false;
                }
            }

            return BfCommand.Eof;
        }
    }
}