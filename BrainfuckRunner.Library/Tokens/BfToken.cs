using System;

namespace BrainfuckRunner.Library.Tokens
{
    public abstract class BfToken
    {
        #region === STATIC STUFF & CONSTANTS ===

        private const string @null = "null";

        /// <summary>
        /// Creates a token from specified Brainfuck command and its position within code
        /// </summary>
        internal static BfToken FromBrainfuckCommand(BfCommand cmd, int pos)
        {
            BfToken token = cmd switch
            {
                BfCommand.MoveBackward => new MoveBackwardBfToken(),
                BfCommand.MoveForward => new MoveForwardBfToken(),
                BfCommand.Decrement => new DecrementBfToken(),
                BfCommand.Increment => new IncrementBfToken(),
                BfCommand.Read => new ReadBfToken(),
                BfCommand.Print => new PrintBfToken(),
                BfCommand.OpenLoop => new LoopBfToken(),
                BfCommand.CloseLoop => new CloseLoopBfToken(),
                _ => null
            };

            if (token != null)
            {
                token.Position = pos;
                return token;
            }

            throw new ArgumentException(
                $"Failed to instantiate a token for specified Brainfuck command: {cmd:G}", 
                nameof(cmd));
        }

        private static string GetSysPrefix(BfToken token)
        {
            return token switch
            {
                RootBfToken _ => "ROOT",
                DecrementBfToken _ => "DEC",
                IncrementBfToken _ => "INC",
                ReadBfToken _ => "READ",
                PrintBfToken _ => "PRINT",
                MoveForwardBfToken _ => "FORWARD",
                MoveBackwardBfToken _ => "BACKWARD",
                LoopBfToken _ => "LOOP",
                CloseLoopBfToken _ => "LOOP-CLOSE",
                PlainTextBfToken txt when txt.IsNewLine => "NEW-LINE",
                PlainTextBfToken txt when txt.IsWhiteSpace => "WHITESPACE",
                PlainTextBfToken _ => "TEXT",
                _ => string.Empty
            };
        }
        #endregion

        private TreeBfToken _parent;
        private int _pos;

        /// <summary>
        /// Gets length of token in count of characters it spans on 
        /// </summary>
        public abstract int Length
        {
            get;
        }

        /// <summary>
        /// Returns parent token
        /// </summary>
        public TreeBfToken Parent
        {
            get => _parent;
            internal set => _parent = value;
        }

        /// <summary>
        /// Returns start position within text
        /// </summary>
        public int Position
        {
            get => _pos;
            internal set => _pos = value;
        }

        protected string ParentSysPrefix
        {
            get
            {
                if (_parent == null)
                {
                    return @null;
                }

                return _parent.SysPrefix;
            }
        }

        /// <summary>
        /// Debug prefix for internal use
        /// </summary>
        internal string SysPrefix
        {
            get
            {
                return GetSysPrefix(this);
            }
        }
    }
}