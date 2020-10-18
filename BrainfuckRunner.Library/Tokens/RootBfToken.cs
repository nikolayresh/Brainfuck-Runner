using System.Linq;

namespace BrainfuckRunner.Library.Tokens
{
    internal sealed class RootBfToken : TreeBfToken
    {
        public override void AcceptChild(BfToken child)
        {
            EnsureChildren().Add(child);
        }

        public override int Length
        {
            get
            {
                return Children.Sum(x => x.Length);
            }
        }

        public override string ToString()
        {
            return $"ROOT (Length: {Length}, Children: {Children.Count})";
        }
    }
}