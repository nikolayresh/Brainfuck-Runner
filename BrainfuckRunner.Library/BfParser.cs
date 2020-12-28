using System.IO;
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
            /// <summary>
            /// Initial state of parser. Reading non-comment content
            /// </summary>
            NonComment = 0,

            /// <summary>
            /// Parser is reading string that starts a comment literal
            /// </summary>
            OpeningComment,

            /// <summary>
            /// Parser is skipping comment body
            /// </summary>
            CommentBody,

            /// <summary>
            /// Parser is reading string that ends a comment literal
            /// </summary>
            ClosingComment
        }


        private readonly StringBuilder _bufferStartTag;
        private readonly StringBuilder _bufferEndTag;
        private readonly BfCommentPattern _comment;
        private readonly TextReader _text;
        private ParserState _nextState;

        internal BfParser(TextReader text, BfCommentPattern comment)
        {
            _comment = comment;
            _text = text;
            _bufferStartTag = new StringBuilder();
            _bufferEndTag = new StringBuilder();
            _nextState = ParserState.NonComment;
        }

        internal BfCommand ParseNextCommand()
        {
            int nextChar;
            BfCommand cmd;

            while (true)
            {
                nextChar = _text.Read();

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

            switch (_nextState)
            {
                case ParserState.NonComment:
                    if (ch == _comment.StartTag[0])
                    {
                        _bufferStartTag.Append(ch);
                        _nextState = string.Equals(_comment.StartTag, _bufferStartTag.ToString())
                            ? ParserState.CommentBody
                            : ParserState.OpeningComment;
                    }
                    break;

                case ParserState.OpeningComment:
                    _bufferStartTag.Append(ch);
                    if (string.Equals(_comment.StartTag, _bufferStartTag.ToString()))
                    {
                        _bufferStartTag.Clear();
                        _nextState = ParserState.CommentBody;
                    }
                    break;

                case ParserState.CommentBody:
                    if (ch == _comment.EndTag[0])
                    {
                        _bufferEndTag.Append(ch);
                        _nextState = string.Equals(_comment.EndTag, _bufferEndTag.ToString())
                            ? ParserState.NonComment
                            : ParserState.ClosingComment;
                    }
                    break;

                case ParserState.ClosingComment:
                    _bufferEndTag.Append(ch);
                    if (string.Equals(_comment.EndTag, _bufferEndTag.ToString()))
                    {
                        _bufferEndTag.Clear();
                        _nextState = ParserState.NonComment;
                    }
                    break;
            }

            return _nextState == ParserState.OpeningComment ||
                   _nextState == ParserState.CommentBody ||
                   _nextState == ParserState.ClosingComment;
        }
    }
}