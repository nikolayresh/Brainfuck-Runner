using System.Collections.Generic;
using System.Linq;

namespace BrainfuckRunner.Library.Tokens
{
    public sealed class PlainTextBfToken : BfToken
    {
        private static readonly HashSet<string> NewLineChars;

        static PlainTextBfToken()
        {
            NewLineChars = new HashSet<string>(
                new[]{ "\r\n", "\r", "\n"});
        }

        private string _text;

        /// <summary>
        /// Gets plain content
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            internal set
            {
                _text = value;
            }
        }

        /// <summary>
        /// Returns a boolean value whether this text segment is
        /// a set of new-line control chars
        /// </summary>
        public bool IsNewLine
        {
            get
            {
                return _text != null
                       && _text.Length <= 2
                       && NewLineChars.Contains(_text);
            }
        }

        /// <summary>
        /// Returns a boolean value whether this text segment
        /// is a whitespace string
        /// </summary>
        public bool IsWhiteSpace
        {
            get
            {
                return _text != null && 
                       _text.Length > 0 && 
                       _text.All(x => char.IsWhiteSpace(x));
            }
        }

        public override int Length
        {
            get
            {
                if (_text != null)
                {
                    return _text.Length;
                }

                return -1;
            }
        }

        public override string ToString()
        {
            return $"{SysPrefix} (Text: \"{Text}\", Length: {Length}, Parent: {ParentSysPrefix})";
        }
    }
}