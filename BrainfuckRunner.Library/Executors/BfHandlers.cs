using BrainfuckRunner.Library.Behaviors;

namespace BrainfuckRunner.Library.Executors
{
    internal delegate void BfCommandHandler(BfEngine engine, ref int iNextCmd);

    internal sealed class BfHandlers
    {
        internal BfHandlers()
        {
            Init();
        }

        private void Init()
        {
            OnMoveBackward = (BfEngine engine, ref int iNextCmd) =>
            {
                engine.Pointer--;
                iNextCmd++;

                if (engine.Pointer >= 0)
                {
                    return;
                }

                engine.Pointer = engine.OnMemoryOverflow switch
                {
                    BfMemoryOverflowBehavior.ThrowError => throw new BfException(
                        BfRuntimeError.OutOfMemoryRange,
                        $"Pointer went out of valid memory range [0 - {engine.TapeSize - 1}]. Position: {engine.Pointer}"),
                    BfMemoryOverflowBehavior.ApplyOverflow => BfExtensions.Mod(engine.Pointer, @base: engine.TapeSize),
                    BfMemoryOverflowBehavior.MovePointerToThreshold => 0,
                    _ => engine.Pointer
                };
            };

            OnMoveForward = (BfEngine engine, ref int iNextCmd) =>
            {
                engine.Pointer++;
                iNextCmd++;

                if (engine.Pointer < engine.TapeSize)
                {
                    return;
                }

                engine.Pointer = engine.OnMemoryOverflow switch
                {
                    BfMemoryOverflowBehavior.ThrowError => throw new BfException(
                        BfRuntimeError.OutOfMemoryRange,
                        $"Pointer went out of valid memory range [0 - {engine.TapeSize - 1}]. Position: {engine.Pointer}"),
                    BfMemoryOverflowBehavior.ApplyOverflow => BfExtensions.Mod(engine.Pointer, @base: engine.TapeSize),
                    BfMemoryOverflowBehavior.MovePointerToThreshold => engine.TapeSize - 1,
                    _ => engine.Pointer
                };
            };

            OnDecrementCell = (BfEngine engine, ref int iNextCmd) =>
            {
                ChangeCell(engine, -1);
                iNextCmd++;
            };

            OnIncrementCell = (BfEngine engine, ref int iNextCmd) =>
            {
                ChangeCell(engine, 1);
                iNextCmd++;
            };

            OnReadIntoCell = (BfEngine engine, ref int iNextCmd) =>
            {
                var nextChar = engine.Input.Read();
                engine.Cells[engine.Pointer] = (byte) nextChar;
                iNextCmd++;
            };

            OnPrintCell = (BfEngine engine, ref int iNextCmd) =>
            {
                var ch = (char) engine.Cells[engine.Pointer];
                engine.Output.Write(ch);
                iNextCmd++;
            };

            OnOpenLoop = (BfEngine engine, ref int iNextCmd) =>
            {
                var ptrValue = engine.Cells[engine.Pointer];

                if (ptrValue == 0)
                {
                    var commands = engine.Commands;
                    var loopDepth = 1;

                    while (loopDepth != 0)
                    {
                        iNextCmd++;

                        switch (commands[iNextCmd])
                        {
                            case BfCommand.OpenLoop:
                                loopDepth++;
                                continue;
                            case BfCommand.CloseLoop:
                                loopDepth--;
                                continue;
                        }
                    }
                } else
                {
                    iNextCmd++;
                }
            };

            OnCloseLoop = (BfEngine engine, ref int iNextCmd) =>
            {
                var ptrValue = engine.Cells[engine.Pointer];

                if (ptrValue != 0)
                {
                    var commands = engine.Commands;
                    var loopDepth = 1;

                    while (loopDepth != 0)
                    {
                        iNextCmd--;

                        switch (commands[iNextCmd])
                        {
                            case BfCommand.CloseLoop:
                                loopDepth++;
                                continue;
                            case BfCommand.OpenLoop:
                                loopDepth--;
                                continue;
                        }
                    }
                } else
                { 
                    iNextCmd++;
                }
            };
        }

        private static void ChangeCell(BfEngine engine, int delta)
        {
            checked
            {
                var (ptr, cells) = engine.GetPointerCellsTuple();
                var ret = cells[ptr] + delta;

                if (ret >= byte.MinValue && ret <= byte.MaxValue)
                {
                    cells[ptr] = (byte) ret;
                    return;
                }

                ret = engine.OnCellOverflow switch
                {
                    BfCellOverflowBehavior.ApplyOverflow => BfExtensions.Mod(ret, @base: 256),
                    BfCellOverflowBehavior.SetThresholdValue => (ret < byte.MinValue) ? byte.MinValue : byte.MaxValue,
                    _ => ret
                };

                cells[ptr] = (byte) ret;
            }
        }

        internal BfCommandHandler OnMoveBackward
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnMoveForward
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnDecrementCell
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnIncrementCell
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnReadIntoCell
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnPrintCell
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnOpenLoop
        {
            get; 
            private set;
        }

        internal BfCommandHandler OnCloseLoop
        {
            get; 
            private set;
        }
    }
}