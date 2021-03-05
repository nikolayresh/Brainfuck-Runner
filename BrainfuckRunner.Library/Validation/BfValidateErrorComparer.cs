using System.Collections.Generic;

namespace BrainfuckRunner.Library.Validation
{
    internal sealed class BfValidateErrorComparer : IComparer<BfValidateError>
    {
        internal static readonly BfValidateErrorComparer Instance = new BfValidateErrorComparer();

        /// <summary>
        /// Private constructor for singleton
        /// </summary>
        private BfValidateErrorComparer()
        {
        }

        public int Compare(BfValidateError x, BfValidateError y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (x.Position < y.Position)
            {
                return -1;
            }

            if (x.Position > y.Position)
            {
                return 1;
            }

            return 0;
        }
    }
}