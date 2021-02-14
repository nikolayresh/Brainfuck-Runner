namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Defines a set of Brainfuck commands along with pseudo ones for internal use
    /// </summary>
    public enum BfCommand : sbyte
    {
        /// <summary>
        /// Pseudo Brainfuck command that defines state when end of file or text resource reached
        /// </summary>
        Eof = -1,

        /// <summary>
        /// Pseudo Brainfuck command that defines initial state
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Defines the MOVE-BACKWARD command, it moves memory pointer to left
        /// </summary>
        MoveBackward = 1,

        /// <summary>
        /// Defines the MOVE-FORWARD command, it moves memory pointer to right
        /// </summary>
        MoveForward = 2,

        /// <summary>
        /// Defines the DECREMENT command, it decrements value by 1 of current cell
        /// </summary>
        Decrement = 3,

        /// <summary>
        /// Defines the INCREMENT command, it increments value by 1 of current cell
        /// </summary>
        Increment = 4,
 
        /// <summary>
        /// Defines the READ command, it reads a character code from user's command prompt
        /// </summary>
        Read = 5,

        /// <summary>
        /// Defines the PRINT command, it prints value of current cell
        /// </summary>
        Print = 6,

        /// <summary>
        /// Defines the OPEN-LOOP command, it starts a new Brainfuck loop construction
        /// </summary>
        OpenLoop = 7,

        /// <summary>
        /// Defines the CLOSE-LOOP command, it ends/closes already opened Brainfuck loop
        /// </summary>
        CloseLoop = 8
    }
}