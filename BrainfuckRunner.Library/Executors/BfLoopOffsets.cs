using System.Collections.Generic;
using System.Linq;
using BrainfuckRunner.Library.Behaviors;

namespace BrainfuckRunner.Library.Executors
{
    internal sealed class BfLoopOffsets
    {
        private readonly int _minOffset;
        private readonly int _maxOffset;

        internal BfLoopOffsets(BfLoop loop, BfEngine engine)
        {
            Map = new Dictionary<int, StructRef<int>>();
            IsCopyPattern = InitializeOffsetsMap(
                loop, engine, 
                ref _minOffset, ref _maxOffset);
        }

        private bool InitializeOffsetsMap(BfLoop loop, BfEngine engine, ref int minOffset, ref int maxOffset)
        {
            var commands = BfLoop.GetLoopCommands(loop, engine);

            if (commands.Length == 0 || commands.Any(x => !x.IsMultiplyLoopCmd()))
            {
                return false;
            }

            Map[0] = new StructRef<int>(0);
            var ptr = 0;

            foreach (var cmd in commands)
            {
                var ptrDelta = 0;
                if (cmd.IsPointerShift(ref ptrDelta, false))
                {
                    ptr += ptrDelta;

                    switch (cmd)
                    {
                        case BfCommand.MoveForward when ptr > maxOffset:
                            maxOffset = ptr;
                            break;

                        case BfCommand.MoveBackward when ptr < minOffset:
                            minOffset = ptr;
                            break;
                    }

                    continue;
                }

                if (!Map.TryGetValue(ptr, out var delta))
                {
                    delta = new StructRef<int>(0);
                    Map[ptr] = delta;
                }

                switch (cmd)
                {
                    case BfCommand.Decrement:
                        delta.Value--;
                        continue;
                    case BfCommand.Increment:
                        delta.Value++;
                        continue;
                }
            }

            bool baseCellWillTurnZero()
            {
                var baseDelta = Map[0].Value;
                return baseDelta == -1 || (engine.OnCellOverflow == BfCellOverflowBehavior.ApplyOverflow && baseDelta == 1);
            }

            return minOffset != maxOffset && 
                   ptr == 0 && 
                   baseCellWillTurnZero();
        }

        /// <summary>
        /// Returns a boolean value whether commands within
        /// loop make up a copy-loop construction
        /// </summary>
        internal bool IsCopyPattern
        {
            get;
        }

        /// <summary>
        /// Returns value of loop offsets
        /// </summary>
        internal Dictionary<int, StructRef<int>> Map
        {
            get;
        }

        /// <summary>
        /// Returns value of the minimal offset
        /// </summary>
        internal int Min
        {
            get => _minOffset;
        }

        /// <summary>
        /// Returns value of the maximal offset
        /// </summary>
        internal int Max
        {
            get => _maxOffset;
        }
    }
}