namespace BrainfuckRunner.Library.Tokens
{
    /// <summary>
    /// Interface that defines token as a valid Brainfuck command
    /// </summary>
    public interface IBrainfuckCommand
    {
        /// <summary>
        /// Returns command type of a Brainfuck token
        /// </summary>
        BfCommand Command { get; }
    }
}