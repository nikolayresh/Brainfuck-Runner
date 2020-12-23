using System.IO;
using System.Linq;
using System.Text;

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
        /// State of parser
        /// </summary>
        internal enum ParserState
        {
            NonComment,
            CommentStart,
            CommentBody,
            CommentEnd
        }

        private readonly BfCommentPattern _comment;
        private readonly StringBuilder _bufferStartTag;
        private readonly StringBuilder _bufferEndTag;
        private ParserState _state;

        internal BfParser(BfCommentPattern comment)
        {
            _comment = comment;
            _bufferStartTag = new StringBuilder();
            _bufferEndTag = new StringBuilder();
            _state = ParserState.NonComment;
        }

        internal BfCommand ParseNextCommand(TextReader text)
        {
            int nextChar;
            BfCommand cmd;

            while (true)
            {
                nextChar = text.Read();

                if (nextChar != Eof)
                {
                    if (IsWithinComment(nextChar))
                    {
                        continue;
                    }

                    if (IsBrainfuckCommand(nextChar, out cmd))
                    {
                        return cmd;
                    }

                    continue;
                }

                return BfCommand.EndOfFile;
            }
        }

        private bool IsWithinComment(int nextChar)
        {
            if (_comment is null)
            {
                return false;
            }

            var ch = (char) nextChar;

            switch (_state)
            {
                case ParserState.NonComment:
                    if (ch == _comment.StartTag.First())
                    {
                        _bufferStartTag.Append(ch);
                        _state = ParserState.CommentStart;
                    }
                    break;

                case ParserState.CommentStart:
                    _bufferStartTag.Append(ch);
                    if (string.Equals(_comment.StartTag, _bufferStartTag.ToString()))
                    {
                        _bufferStartTag.Clear();
                        _state = ParserState.CommentBody;
                    }
                    break;

                case ParserState.CommentBody:
                    if (ch == _comment.EndTag.First())
                    {
                        _bufferEndTag.Append(ch);
                        _state = ParserState.CommentEnd;
                    }
                    break;

                case ParserState.CommentEnd:
                    _bufferEndTag.Append(ch);
                    if (string.Equals(_comment.EndTag, _bufferEndTag.ToString()))
                    {
                        _bufferEndTag.Clear();
                        _state = ParserState.NonComment;
                    }
                    break;
            }

            return _state == ParserState.CommentStart ||
                   _state == ParserState.CommentBody ||
                   _state == ParserState.CommentEnd;
        }
    }
}