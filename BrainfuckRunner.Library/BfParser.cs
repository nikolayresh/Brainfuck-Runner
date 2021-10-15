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

        private TextReader _text;
        private string _commentToken;
        private int _loopState;

        internal void SetState(TextReader text, string commentToken)
        {
            _text = text;
            _commentToken = commentToken;
            _loopState = 0;
        }

        /// <summary>
        /// Returns next parsed Brainfuck command
        /// </summary>
        internal BfCommand[] ParseCommands()
        {
            List<BfCommand> parsedCommands = new();

            string line;
            while ((line = _text.ReadLine()) != null)
            {
                int stopIndex = line.Length;

                if (_commentToken != null)
                {
                    int commentIndex = line.IndexOf(_commentToken, StringComparison.InvariantCulture);
                    if (commentIndex >= 0) stopIndex = commentIndex;
                }

                int i = 0;
                while (i < stopIndex)
                {
                    if (IsBrainfuckCommand(line[i++], out BfCommand cmd))
                    {
                        parsedCommands.Add(cmd);
                        cmd.TryChangeLoopsRef(ref _loopState);
                    }
                }
            }

            return parsedCommands.ToArray();
        }

        internal int LoopState
        {
            get
            {
                return _loopState;
            }
        }
    }
}