using System.IO;
using System.Text;

namespace BrainfuckRunner.Library
{
    internal static class BfParser
    {
        /// <summary>
        /// Defines state when end of file reached
        /// </summary>
        internal const int Eof = -1;

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

        /// <summary>
        /// Returns a boolean value whether specified character
        /// is a valid Brainfuck command
        /// </summary>
        internal static bool IsBrainfuckCommand(int arg, out BfCommand cmd)
        {
            cmd = BfCommand.Unknown;
            var ch = (char) arg;

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

        /// <summary>
        /// Returns a next parsed Brainfuck command
        /// </summary>
        internal static BfCommand ParseNextCommand(TextReader text, ref int loops)
        {
            int nextChar;
            BfCommand cmd;

            while (true)
            {
                nextChar = text.Read();

                if (nextChar != Eof)
                {
                    if (IsBrainfuckCommand(nextChar, out cmd))
                    {
                        cmd.TryChangeLoopsRef(ref loops);
                        return cmd;
                    }

                    continue;
                }

                return BfCommand.Eof;
            }
        }
    }
}