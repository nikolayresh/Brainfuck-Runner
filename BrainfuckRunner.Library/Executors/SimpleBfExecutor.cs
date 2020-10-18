namespace BrainfuckRunner.Library.Executors
{
    internal sealed class SimpleBfExecutor : BfExecutor
    {
        private static readonly BfHandlers Handlers = new BfHandlers();

        internal SimpleBfExecutor(BfEngine engine) : base(engine)
        {
        }

        internal override void RunCommand(BfCommand cmd, ref int iNextCmd)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                    Handlers.OnMoveBackward(Engine, ref iNextCmd);
                    break;

                case BfCommand.MoveForward:
                    Handlers.OnMoveForward(Engine, ref iNextCmd);
                    break;

                case BfCommand.Decrement:
                    Handlers.OnDecrementCell(Engine, ref iNextCmd);
                    break;

                case BfCommand.Increment:
                    Handlers.OnIncrementCell(Engine, ref iNextCmd);
                    break;

                case BfCommand.Read:
                    Handlers.OnReadIntoCell(Engine, ref iNextCmd);
                    break;

                case BfCommand.Print:
                    Handlers.OnPrintCell(Engine, ref iNextCmd);
                    break;

                case BfCommand.OpenLoop:
                    Handlers.OnOpenLoop(Engine, ref iNextCmd);
                    break;

                case BfCommand.CloseLoop:
                    Handlers.OnCloseLoop(Engine, ref iNextCmd);
                    break;
            }
        }
    }
}