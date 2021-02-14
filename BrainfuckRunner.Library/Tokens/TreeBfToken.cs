using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrainfuckRunner.Library.Tokens
{
    public abstract class TreeBfToken : BfToken
    {
        private static readonly ReadOnlyCollection<BfToken> NoChildren;

        static TreeBfToken()
        {
            NoChildren = new ReadOnlyCollection<BfToken>(new List<BfToken>());
        }

        private List<BfToken> _children;

        /// <summary>
        /// Accepts a child token
        /// </summary>
        public virtual void AcceptChild(BfToken child)
        {
            child.Parent = this;

            EnsureChildren().Add(child);
        }

        /// <summary>
        /// Ensures & returns internal list of child tokens
        /// </summary>
        protected List<BfToken> EnsureChildren()
        {
            return _children ??= new List<BfToken>();
        }

        /// <summary>
        /// Returns collection of child tokens
        /// </summary>
        public ReadOnlyCollection<BfToken> Children
        {
            get
            {
                if (_children == null)
                {
                    return NoChildren;
                }

                return _children.AsReadOnly();
            }
        }
    }
}