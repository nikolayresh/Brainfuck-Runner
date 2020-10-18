using System.Collections.Generic;
using System.Linq;

namespace BrainfuckRunner.Library.Validation
{
    /// <summary>
    /// Result of Brainfuck code validation
    /// </summary>
    public sealed class BfValidateResult
    {
        private readonly SortedSet<BfValidateError> _errors;

        public BfValidateResult()
        {
            _errors = new SortedSet<BfValidateError>(BfValidateErrorComparer.Instance);
        }

        /// <summary>
        /// Gets set of errors for internal use
        /// </summary>
        internal SortedSet<BfValidateError> GetErrorsList()
        {
            return _errors;
        }

        /// <summary>
        /// Gets validation errors
        /// </summary>
        public BfValidateError[] Errors
        {
            get
            {
                return _errors.ToArray();
            }
        }

        /// <summary>
        /// Gets a boolean value whether validator found any errors
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _errors.Count == 0;
            }
        }
    }
}