using System;
using System.Collections.Generic;
using System.Linq;
using BrainfuckRunner.Library.Behaviors;

namespace BrainfuckRunner.Library.Executors
{
    internal sealed class OptimizedBfExecutor : BfExecutor
    {
        private delegate void OptimizedHandler(ref int iNextCmd);

        private readonly Dictionary<int, BfLoop> _loopsCache;
        private readonly OptimizedHandler _onSetPointerPosition;
        private readonly OptimizedHandler _onChangeCell;

        internal OptimizedBfExecutor(BfEngine engine) : base(engine)
        {
            _loopsCache = new Dictionary<int, BfLoop>();
            _onSetPointerPosition = ResolveHandlerOnSetPointerPosition(engine);
            _onChangeCell = ResolveHandlerOnChangeCell(engine);
        }

        internal override void Initialize()
        {
            Stack<BfLoop> loopsTree = new Stack<BfLoop>();
            var loopCommands = Engine.Commands
                .Select((cmd, pos) => new
                 {
                    Command = cmd, 
                    Position = pos
                 }).Where(x => x.Command.IsLoopCommand());

            foreach (var loopCmd in loopCommands)
            {
                switch (loopCmd.Command)
                {
                    case BfCommand.OpenLoop:
                        if (loopsTree.TryPeek(out BfLoop parent))
                        {
                            parent.SubLoops++;
                        }

                        loopsTree.Push(new BfLoop
                        {
                            StartPosition = loopCmd.Position,
                            EndPosition = -1,
                            Offsets = default,
                            SubLoops = default,
                            ScanStep = default,
                            ZeroState = default
                        });

                        _loopsCache[loopCmd.Position] = loopsTree.Peek();
                        continue;

                    case BfCommand.CloseLoop:
                        BfLoop loop = loopsTree.Pop();
                        loop.EndPosition = loopCmd.Position;
                        _loopsCache[loopCmd.Position] = loop;
                        continue;
                }   
            }
        }

        internal override void RunCommand(BfCommand cmd, ref int iNextCmd)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                case BfCommand.MoveForward:
                    _onSetPointerPosition(ref iNextCmd);
                    break;

                case BfCommand.Decrement:
                case BfCommand.Increment:
                    _onChangeCell(ref iNextCmd);
                    break;

                case BfCommand.Read:
                    ReadIntoCell(ref iNextCmd);
                    break;

                case BfCommand.Print:
                    PrintCell(ref iNextCmd);
                    break;

                case BfCommand.OpenLoop:
                    OnLoopOpening(ref iNextCmd);
                    break;

                case BfCommand.CloseLoop:
                    OnLoopClosing(ref iNextCmd);
                    break;
            }
        }

        private OptimizedHandler ResolveHandlerOnSetPointerPosition(BfEngine engine)
        {
            return engine.OnMemoryOverflow switch
            {
                BfMemoryOverflowBehavior.ThrowError => SetPointerWithDefaultBehavior,
                BfMemoryOverflowBehavior.ApplyOverflow => SetPointerWithOverflowBehavior,
                BfMemoryOverflowBehavior.MovePointerToThreshold => SetPointerWithThresholdBehavior,
                _ => throw new ArgumentException(
                    $"Failed to resolve a callback function for specified behavior: {engine.OnMemoryOverflow:G}", 
                    nameof(engine.OnMemoryOverflow))
            };
        }

        private OptimizedHandler ResolveHandlerOnChangeCell(BfEngine engine)
        {
            return engine.OnCellOverflow switch
            {
                BfCellOverflowBehavior.ApplyOverflow => ChangeCellWithOverflowBehavior,
                BfCellOverflowBehavior.SetThresholdValue => ChangeCellWithThresholdBehavior,
                _ => throw new ArgumentException(
                     $"Failed to resolve a callback function for specified behavior: {engine.OnCellOverflow:G}",
                     nameof(engine.OnCellOverflow))
            };
        }

        private void SetPointerWithDefaultBehavior(ref int iNextCmd)
        {
            (int ptr, BfCommand[] commands, int tapeSize) = Engine.GetBaseTuple();
            int iCmd = iNextCmd;
            int delta = 0;
            bool isOutOfRange = false;

            while (iCmd < commands.Length && commands[iCmd].IsPointerShift(ref delta, false))
            {
                ptr += delta;
                iCmd++;

                if (ptr < 0 || ptr >= tapeSize)
                {
                    isOutOfRange = true;
                    break;
                }
            }

            Engine.Pointer = ptr;
            iNextCmd = iCmd;

            if (isOutOfRange)
            {
                throw new BfException(
                    BfRuntimeError.OutOfMemoryRange,
                    $"Pointer went out of valid memory range [0 - {tapeSize - 1}]. Position: {Engine.Pointer}");
            }
        }

        private void SetPointerWithThresholdBehavior(ref int iNextCmd)
        {
            (int ptr, BfCommand[] commands, int tapeSize) = Engine.GetBaseTuple();
            int iCmd = iNextCmd;
            int delta = 0;

            while (iCmd < commands.Length && commands[iCmd].IsPointerShift(ref delta, false))
            {
                ptr += delta;
                iCmd++;

                if (ptr < 0 || ptr >= tapeSize)
                {
                    ptr = (ptr < 0)
                        ? 0
                        : (tapeSize - 1);
                }
            }

            Engine.Pointer = ptr;
            iNextCmd = iCmd;
        }

        private void SetPointerWithOverflowBehavior(ref int iNextCmd)
        {
            (int ptr, BfCommand[] commands, int tapeSize) = Engine.GetBaseTuple();
            int iCmd = iNextCmd;
            int delta = 0;

            while (iCmd < commands.Length && commands[iCmd].IsPointerShift(ref delta, true))
            {
                iCmd++;
            }

            ptr += delta;

            Engine.Pointer = BfExtensions.Mod(ptr, @base: tapeSize);
            iNextCmd = iCmd;
        }

        private void ChangeCellWithOverflowBehavior(ref int iNextCmd)
        {
            (int ptr, BfCommand[] commands, byte[] cells) = Engine.GetCellsTuple();
            int iCmd = iNextCmd;
            int delta = 0;

            while (iCmd < commands.Length && commands[iCmd].IsCellChanger(ref delta, true))
            {
                iCmd++;
            }

            cells[ptr] = ApplyDeltaOnCell(cells[ptr], delta);
            iNextCmd = iCmd;
        }

        private void ChangeCellWithThresholdBehavior(ref int iNextCmd)
        {
            int ptrValue = Engine.Cells[Engine.Pointer];
            BfCommand[] commands = Engine.Commands;
            int iCmd = iNextCmd;
            int delta = 0;

            while (iCmd < commands.Length && commands[iCmd].IsCellChanger(ref delta, false))
            {
                ptrValue += delta;
                iCmd++;

                if (ptrValue is < byte.MinValue or > byte.MaxValue)
                {
                    ptrValue = (ptrValue < byte.MinValue)
                        ? byte.MinValue
                        : byte.MaxValue;
                }
            }

            Engine.Cells[Engine.Pointer] = (byte) ptrValue;
            iNextCmd = iCmd;
        }

        private void PrintCell(ref int iNextCmd)
        {
            char ch = (char) Engine.Cells[Engine.Pointer];
            BfCommand[] commands = Engine.Commands;
            int iCmd = iNextCmd;

            while (iCmd < commands.Length && commands[iCmd] == BfCommand.Print)
            {
                iCmd++;
            }

            int times = iCmd - iNextCmd;
            iNextCmd = iCmd;

            while (times-- != 0)
            {
                Engine.Output.Write(ch);
            }
        }

        private void OnLoopOpening(ref int iNextCmd)
        {
            byte ptrValue = Engine.Cells[Engine.Pointer];
            BfLoop loop = _loopsCache[iNextCmd];

            if (ptrValue == 0)
            {
                iNextCmd = loop.EndPosition + 1;
                return;
            }

            if (loop.SubLoops == 0)
            {
                if (IsZeroLoop(loop))
                {
                    Engine.Cells[Engine.Pointer] = 0;
                    iNextCmd = loop.EndPosition + 1;
                    return;
                }

                if (IsScanLoop(loop, out int ptr))
                {
                    Engine.Pointer = ptr;
                    iNextCmd = loop.EndPosition + 1;
                    return;
                }

                if (IsMultiplyLoop(loop, out BfLoopOffsets offsets))
                {
                    MultiplyLoopAction(offsets);
                    iNextCmd = loop.EndPosition + 1;
                    return;
                }
            }

            iNextCmd++;
        }

        private void MultiplyLoopAction(BfLoopOffsets offsets)
        {
            (int ptr, byte[] cells) = Engine.GetPointerCellsTuple();
            Dictionary<int, StructRef<int>> map = offsets.Map;

            while (cells[ptr] != 0)
            {
                foreach (int offset in map.Keys)
                {
                    int delta = map[offset].Value;
                    int pos = ptr + offset;
                    cells[pos] = ApplyDeltaOnCell(cells[pos], delta);
                }
            }
        }

        private bool IsScanLoop(BfLoop loop, out int ptr)
        {
            ptr = Engine.Pointer;

            if (loop.ScanStep != null && loop.ScanStep == 0)
            {
                // not a scan loop
                return false;
            }

            if (loop.ScanStep == null)
            {
                BfCommand[] commands = Engine.Commands;
                int iCmd = loop.StartPosition + 1;
                BfCommand cmd;

                if (loop.IsEmpty || !(cmd = commands[iCmd]).IsPointerShift())
                {
                    // loop should consist of at least 
                    // a single pointer-shift command within it
                    loop.ScanStep = 0;
                    return false;
                }

                int posEnd = loop.EndPosition;
                iCmd++;

                if (iCmd != posEnd)
                {
                    while (iCmd < posEnd && commands[iCmd] == cmd)
                    {
                        iCmd++;
                    }

                    if (iCmd != posEnd)
                    {
                        // body of loop contains 
                        // commands other than the first one 
                        loop.ScanStep = 0;
                        return false;
                    }
                }

                loop.ScanStep = loop.ContentLength * (cmd == BfCommand.MoveBackward ? -1 : 1);
            }

            byte[] cells = Engine.Cells;
            int step = Math.Abs(loop.ScanStep.Value);

            if (loop.ScanStep < 0)
            {
                while (ptr >= 0 && cells[ptr] != 0)
                {
                    ptr -= step;
                }
            } else
            {
                while (ptr < cells.Length && cells[ptr] != 0)
                {
                    ptr += step;
                }
            }

            return ptr >= 0 && ptr < cells.Length;
        }

        private bool IsZeroLoop(BfLoop loop)
        {
            if (loop.ZeroState != null)
            {
                // zero-state of specified loop is already 
                // calculated on previous iteration
                return loop.ZeroState.Value;
            }

            if (Engine.OnCellOverflow != BfCellOverflowBehavior.ApplyOverflow)
            {
                // loop will turn cell value to 0 (zero) if it's
                // a DECREMENT command only, as INCREMENT will reach
                // value 255 and will not overflow to 0 (zero)
                loop.ZeroState = loop.ContentLength == 1 &&
                                 Engine.Commands[loop.StartPosition + 1] == BfCommand.Decrement;
                return loop.ZeroState.Value;
            }

            BfCommand[] commands = Engine.Commands;
            int iCmd = loop.StartPosition + 1;
            int posEnd = loop.EndPosition;
            int delta = 0;

            while (iCmd < posEnd && commands[iCmd].IsCellChanger(ref delta, true))
            {
                iCmd++;
            }

            loop.ZeroState = iCmd == posEnd && Math.Abs(delta) == 1;

            return loop.ZeroState.Value;
        }

        private void OnLoopClosing(ref int iNextCmd)
        {
            byte ptrValue = Engine.Cells[Engine.Pointer];

            if (ptrValue != 0)
            {
                BfLoop loop = _loopsCache[iNextCmd];
                iNextCmd = loop.StartPosition + 1;
            } else
            {
                iNextCmd++;
            }
        }

        private void ReadIntoCell(ref int iNextCmd)
        {
            int nextChar = Engine.Input.Read();
            Engine.Cells[Engine.Pointer] = (byte) nextChar;
            iNextCmd++;
        }

        private bool IsMultiplyLoop(BfLoop loop, out BfLoopOffsets offsets)
        {
            offsets = loop.Offsets ?? (loop.Offsets = new BfLoopOffsets(loop, Engine));
            int ptr = Engine.Pointer;

            return
                offsets.IsCopyPattern &&
                ptr + offsets.Min >= 0 &&
                ptr + offsets.Max < Engine.TapeSize;
        }

        private byte ApplyDeltaOnCell(byte current, int delta)
        {
            unchecked
            {
                int ret = current + delta;

                if (ret is >= byte.MinValue and <= byte.MaxValue)
                {
                    return (byte) ret;
                }

                ret = Engine.OnCellOverflow switch
                {
                    BfCellOverflowBehavior.ApplyOverflow => BfExtensions.Mod(ret, @base: 256),
                    BfCellOverflowBehavior.SetThresholdValue => (ret < byte.MinValue) ? byte.MinValue : byte.MaxValue,
                    _ => ret
                };

                return (byte) ret;
            }
        }
    }
}