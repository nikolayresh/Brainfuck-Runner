namespace BrainfuckRunner.Library.Behaviors
{
    /// <summary>
    /// Set of options used to resolve cases when value of cell is about to overflow
    /// </summary>
    public enum BfCellOverflowBehavior
    {
        /// <summary>
        /// Apply circular overflow.
        /// For instance, if current value of cell is 255 (max value of a byte)
        /// followed by an increment command, will result in cell value set to 0 (zero).
        /// Otherwise, if current value of cell is 0 (zero), followed by
        /// a decrement command, will result in value of cell set to 255
        /// </summary>
        ApplyOverflow,

        /// <summary>
        /// Set closest threshold value within range of a byte from 0 to 255.
        /// For instance, if current value of cell is 254 followed by 3 increment
        /// commands, will result in cell value set to 255 (max value for a byte).
        /// Otherwise, if current value of cell is 5 followed by 7 decrement commands,
        /// will result in value of cell set to 0 (min value for a byte) 
        /// </summary>
        SetThresholdValue
    }
}
