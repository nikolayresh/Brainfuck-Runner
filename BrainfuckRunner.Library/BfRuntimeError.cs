namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Code of runtime error that is thrown while Brainfuck code is being read or executed
    /// </summary>
    public enum BfRuntimeError
    {
        /// <summary>
        /// Error that is thrown when the tape pointer goes out of valid memory range 
        /// </summary>
        OutOfMemoryRange,

        /// <summary>
        /// Error that is thrown when the Brainfuck engine found at least
        /// a single unopened loop 
        /// </summary>
        HasUnopenedLoops,

        /// <summary>
        /// Error that is thrown when the Brainfuck engine found at least
        /// a single unclosed loop
        /// </summary>
        HasUnclosedLoops
    }
}