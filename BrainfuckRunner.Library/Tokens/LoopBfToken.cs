using System.Linq;

namespace BrainfuckRunner.Library.Tokens
{
    public sealed class LoopBfToken : TreeBfToken, IBrainfuckCommand
    {
        private CloseLoopBfToken _closeToken;

        public override void AcceptChild(BfToken child)
        {
            if (child is CloseLoopBfToken token)
            {
                token.Parent = this;
                _closeToken = token;
                return;
            }

            base.AcceptChild(child);
        }

        /// <summary>
        /// Gets a token that closes this loop.
        /// </summary>
        public CloseLoopBfToken CloseToken
        {
            get
            {
                return _closeToken;
            }
        }

        /// <summary>
        /// Gets a boolean value whether this loop is closed
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return _closeToken != null && 
                       ReferenceEquals(_closeToken.Parent, this);
            }
        }

        public override int Length
        {
            get
            {
                var length = 1;

                length += Children.Sum(x => x.Length);

                if (_closeToken != null)
                {
                    length += _closeToken.Length;
                }

                return length;
            }
        }

        public BfCommand Command
        {
            get
            {
                return BfCommand.OpenLoop;
            }
        }

        public override string ToString()
        {
            return $"{SysPrefix} (Length: {Length}, Children: {Children.Count}, Parent: {ParentSysPrefix}, Is Closed: {IsClosed})";
        }
    }
}