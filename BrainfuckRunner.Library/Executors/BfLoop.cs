﻿using System.Linq;
using System.Runtime.CompilerServices;

namespace BrainfuckRunner.Library.Executors
{
    /// <summary>
    /// Brainfuck loop entity
    /// </summary>
    internal sealed class BfLoop
    {
        /// <summary>
        /// Returns commands within body a specified Brainfuck loop (with open/close commands excluded)
        /// </summary>
        internal static BfCommand[] GetLoopCommands(BfLoop loop, BfEngine engine)
        {
            return engine.Commands
                .Skip(loop.StartPosition + 1)
                .Take(loop.ContentLength)
                .ToArray();
        }

        private int _startPos;
        private int _endPos;
        private bool? _zeroState;
        private int? _scanStep;
        private BfLoopOffsets _offsets;
        private int _subLoops;

        /// <summary>
        /// Gets or sets start position of Brainfuck loop
        /// </summary>
        internal int StartPosition
        {
            get => _startPos;
            set => _startPos = value;
        }

        /// <summary>
        /// Gets or sets end position of Brainfuck loop
        /// </summary>
        internal int EndPosition
        {
            get => _endPos;
            set => _endPos = value;
        }

        /// <summary>
        /// Gets or sets zero-state of this loop.
        /// Zero-state stands for loop that results in value
        /// of current cell eventually set to 0 (zero)
        /// </summary>
        internal bool? ZeroState
        {
            get => _zeroState;
            set => _zeroState = value;
        }

        /// <summary>
        /// Gets or sets scan step of this loop
        /// </summary>
        internal int? ScanStep
        {
            get => _scanStep;
            set => _scanStep = value;
        }

        /// <summary>
        /// Returns map of relative offsets
        /// made by inner pointer-shift & cell-changer commands
        /// </summary>
        internal BfLoopOffsets Offsets
        {
            get => _offsets;
            set => _offsets = value;
        }

        /// <summary>
        /// Returns count of commands within body of this loop
        /// </summary>
        internal int ContentLength
        {
            get => _endPos - _startPos - 1;
        }

        /// <summary>
        /// Returns a boolean value whether body
        /// of this loop contains any commands
        /// </summary>
        internal bool IsEmpty
        {
            get => (_endPos - _startPos) == 1;
        }

        /// <summary>
        /// Returns count of inner loops within body of this Brainfuck loop
        /// </summary>
        internal int SubLoops
        {
            get => _subLoops;
            set => _subLoops = value;
        }
    }
}