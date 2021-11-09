using System.Diagnostics.CodeAnalysis;
using BrainfuckRunner.Library.Behaviors;

namespace BrainfuckRunner.Library.Executors
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
    internal sealed class BfSimpleHandlers
    {
        private readonly BfEngine _engine;

        internal BfSimpleHandlers(BfEngine engine)
        {
            _engine = engine;
        }

        private static void ChangeCell(BfEngine engine, int delta)
        {
            checked
            {
                (int ptr, byte[] cells) = engine.GetPointerCellsTuple();
                int ret = cells[ptr] + delta;

                if (ret is >= byte.MinValue and <= byte.MaxValue)
                {
                    cells[ptr] = (byte) ret;
                    return;
                }

                ret = engine.OnCellOverflow switch
                {
                    BfCellOverflowBehavior.ApplyOverflow => BfExtensions.Mod(ret, @base: 256),
                    BfCellOverflowBehavior.SetThresholdValue => ret < byte.MinValue ? byte.MinValue : byte.MaxValue,
                    _ => ret
                };

                cells[ptr] = (byte) ret;
            }
        }

        internal void MoveBackward(ref int iNextCmd)
        {
            _engine.Pointer--;
            iNextCmd++;

            if (_engine.Pointer >= 0)
            {
                return;
            }

            _engine.Pointer = _engine.OnMemoryOverflow switch
            {
                BfMemoryOverflowBehavior.ThrowError => throw new BfException(
                    BfRuntimeError.OutOfMemoryRange,
                    $"Pointer went out of valid memory range [0 - {_engine.TapeSize - 1}]. Position: {_engine.Pointer}"),
                BfMemoryOverflowBehavior.ApplyOverflow => BfExtensions.Mod(_engine.Pointer, @base: _engine.TapeSize),
                BfMemoryOverflowBehavior.MovePointerToThreshold => 0,
                _ => _engine.Pointer
            };
        }

        internal void MoveForward(ref int iNextCmd)
        {
            _engine.Pointer++;
            iNextCmd++;

            if (_engine.Pointer < _engine.TapeSize)
            {
                return;
            }

            _engine.Pointer = _engine.OnMemoryOverflow switch
            {
                BfMemoryOverflowBehavior.ThrowError => throw new BfException(
                    BfRuntimeError.OutOfMemoryRange,
                    $"Pointer went out of valid memory range [0 - {_engine.TapeSize - 1}]. Position: {_engine.Pointer}"),
                BfMemoryOverflowBehavior.ApplyOverflow => BfExtensions.Mod(_engine.Pointer, @base: _engine.TapeSize),
                BfMemoryOverflowBehavior.MovePointerToThreshold => _engine.TapeSize - 1,
                _ => _engine.Pointer
            };
        }

        internal void DecrementCell(ref int iNextCmd)
        {
            ChangeCell(_engine, -1);
            iNextCmd++;
        }

        internal void IncrementCell(ref int iNextCmd)
        {
            ChangeCell(_engine, 1);
            iNextCmd++;
        }

        internal void ReadIntoCell(ref int iNextCmd)
        {
            int @char = _engine.Input.Read();
            if (@char != BfParser.Eof)
            {
                _engine.Cells[_engine.Pointer] = (byte) @char;
            }
            iNextCmd++;
        }

        internal void PrintCell(ref int iNextCmd)
        {
            char ch = (char) _engine.Cells[_engine.Pointer];
            _engine.Output.Write(ch);
            iNextCmd++;
        }

        internal void OpenLoop(ref int iNextCmd)
        {
            byte ptrValue = _engine.Cells[_engine.Pointer];

            if (ptrValue == 0)
            {
                BfCommand[] commands = _engine.Commands;
                int loopDepth = 1;

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
            }
            else
            {
                iNextCmd++;
            }
        }

        internal void CloseLoop(ref int iNextCmd)
        {
            byte ptrValue = _engine.Cells[_engine.Pointer];

            if (ptrValue != 0)
            {
                BfCommand[] commands = _engine.Commands;
                int loopDepth = 1;

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
            }
            else
            {
                iNextCmd++;
            }
        }
    }
}