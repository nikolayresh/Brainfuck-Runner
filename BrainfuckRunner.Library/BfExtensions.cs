using System.Runtime.CompilerServices;

namespace BrainfuckRunner.Library
{
    /// <summary>
    /// Useful set of Brainfuck extensions
    /// </summary>
    internal static class BfExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsCellChanger(this BfCommand cmd, ref int value, bool modify)
        {
            switch (cmd)
            {
                case BfCommand.Decrement:
                    value = modify ? (value - 1) : -1;
                    return true;
                case BfCommand.Increment:
                    value = modify ? (value + 1) : 1;
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsPointerShift(this BfCommand cmd, ref int delta, bool modify)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                    delta = modify ? (delta - 1) : -1;
                    return true;
                case BfCommand.MoveForward:
                    delta = modify ? (delta + 1) : 1;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a boolean value whether specified Brainfuck command
        /// is a pointer-shift command
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsPointerShift(this BfCommand cmd)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                case BfCommand.MoveForward:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a boolean value whether specified Brainfuck command
        /// makes up a correct multiply-loop construction 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsMultiplyLoopCmd(this BfCommand cmd)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                case BfCommand.MoveForward:
                case BfCommand.Decrement:
                case BfCommand.Increment:
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsLoopCmd(this BfCommand cmd)
        {
            switch (cmd)
            {
                case BfCommand.OpenLoop:
                case BfCommand.CloseLoop:
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TryChangeLoopsRef(this BfCommand cmd, ref int loops)
        {
            switch (cmd)
            {
                case BfCommand.OpenLoop:
                    loops++;
                    break;
                case BfCommand.CloseLoop:
                    loops--;
                    break;
            }
        }

        /// <summary>
        /// Calculates modulo of a specified number with respect to base threshold 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Mod(int value, int @base)
        {
            return (@base + (value % @base)) % @base;
        }
    }
}