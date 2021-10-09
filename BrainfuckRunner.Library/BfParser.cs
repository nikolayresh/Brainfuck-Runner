using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace BrainfuckRunner.Library
{
    internal sealed class BfParser
    {
        /// <summary>
        /// Defines state when end of file reached
        /// </summary>
        internal const int Eof = -1;

        /// <summary>
        /// Defines a line-feed control character, char(10)
        /// </summary>
        internal const int LineFeed = 10;

        /// <summary>
        /// Defines a carriage-return control character, char(13)
        /// </summary>
        internal const int CarriageReturn = 13;

        /// <summary>
        /// Defines a white-space control character, char(32)
        /// </summary>
        internal const int WhiteSpace = 32;

        /// <summary>
        /// All Brainfuck commands placed into set
        /// </summary>
        internal static readonly string CommandSet = $"{MoveBackwardCmd}{MoveForwardCmd}{DecrementCmd}{IncrementCmd}{ReadCmd}{PrintCmd}{OpenLoopCmd}{CloseLoopCmd}";

        internal const char MoveBackwardCmd = '<';
        internal const char MoveForwardCmd = '>';
        internal const char DecrementCmd = '-';
        internal const char IncrementCmd = '+';
        internal const char ReadCmd = ',';
        internal const char PrintCmd = '.';
        internal const char OpenLoopCmd = '[';
        internal const char CloseLoopCmd = ']';

        private readonly TextReader _text;
        private readonly HashSet<char> _commentTokens;

        internal BfParser(TextReader text, HashSet<char> commentTokens)
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

        private bool SkipWhiteSpace()
        {
            while (_text.Peek() == WhiteSpace)
            {
                _text.Read();
            }

            return _text.Peek() != Eof;
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

            while (_text.Peek() != Eof && SkipWhiteSpace())
            {
                if (!withinComment)
                {
                    if (IsBrainfuckCommand(_text.Peek(), out BfCommand cmd))
                    {
                        _text.Read();
                        return cmd;
                    }

                    withinComment = _commentTokens != null && _commentTokens.Contains((char) _text.Peek());
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