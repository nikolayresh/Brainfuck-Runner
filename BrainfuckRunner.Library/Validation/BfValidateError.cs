using System;

namespace BrainfuckRunner.Library.Validation
{
    /// <summary>
    /// Descriptor of validation error
    /// </summary>
    public sealed class BfValidateError
    {
        /// <summary>
        /// Returns code of validation error
        /// </summary>
        public BfValidateErrorCode Code
        {
            get;
            internal init;
        }

        /// <summary>
        /// Returns content of error if applicable
        /// </summary>
        public string Content
        {
            get; 
            internal init;
        }

        /// <summary>
        /// Position of validation error
        /// </summary>
        public int Position
        {
            get; 
            internal init;
        }

        /// <summary>
        /// Gets length of invalid token or content in characters
        /// </summary>
        public int Length
        {
            get; 
            internal init;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            BfValidateError other = obj as BfValidateError;

            return Code == other.Code
                   && string.Equals(Content, other.Content)
                   && Position == other.Position
                   && Length == other.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Content, Position, Length);
        }

        public override string ToString()
        {
            return $"{Code:G} @ position {Position}, length: {Length}";
        }
    }
}