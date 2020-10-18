namespace BrainfuckRunner.Library.Validation
{
    /// <summary>
    /// Code of validation error
    /// </summary>
    public enum BfValidateErrorCode
    {
        /// <summary>
        /// Non-Brainfuck content encountered
        /// </summary>
        NonBrainfuckContent,

        /// <summary>
        /// Whitespace content encountered
        /// </summary>
        WhiteSpaceContent,

        /// <summary>
        /// New-line character(s) encountered
        /// </summary>
        NewLine,

        /// <summary>
        /// Open token of a Brainfuck loop was not found
        /// </summary>
        NoOpenLoopToken,

        /// <summary>
        /// Close token of a Brainfuck loop was not found
        /// </summary>
        NoCloseLoopToken
    }
}