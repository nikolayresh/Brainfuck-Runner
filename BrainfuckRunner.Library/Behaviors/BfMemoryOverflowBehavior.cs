namespace BrainfuckRunner.Library.Behaviors
{
    /// <summary>
    /// Set of options used to resolve cases when program pointer goes out of valid memory range
    /// </summary>
    public enum BfMemoryOverflowBehavior
    {
        /// <summary>
        /// Break program execution by throwing exception
        /// </summary>
        ThrowError,

        /// <summary>
        /// Apply circular overflow.
        /// For instance, if size of tape is 30,000 cells and the pointer is located
        /// at position 0 (zero) followed by a move-backward command, will result in
        /// pointer located at position 29,999 (first position from end).
        /// Otherwise, if the pointer is located at position 29,999 followed by
        /// a move-forward command, will result in pointer located at position 0 (zero)
        /// </summary>
        ApplyOverflow,

        /// <summary>
        /// Move memory pointer to the closest threshold of tape.
        /// For instance, if size of tape is 30,000 cells and the pointer is located
        /// at position 29,995 followed by 10 move-forward commands, will result
        /// in pointer located at position 29,999 (the last position in tape).
        /// Otherwise, if the pointer is located at position 5 followed by 10 move-backward commands,
        /// will result in pointer located at position 0 (zero, the first position in tape)
        /// </summary>
        MovePointerToThreshold
    }
}
