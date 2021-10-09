namespace BrainfuckRunner.Library.Executors
{
    internal sealed class SimpleBfExecutor : BfExecutor
    {
        private readonly BfSimpleHandlers _handlers;

        internal SimpleBfExecutor(BfEngine engine) : base(engine)
        {
            _handlers = new BfSimpleHandlers(engine);
        }

        internal override void RunCommand(BfCommand cmd, ref int iNextCmd)
        {
            switch (cmd)
            {
                case BfCommand.MoveBackward:
                    _handlers.MoveBackward(ref iNextCmd);
                    break;

                case BfCommand.MoveForward:
                    _handlers.MoveForward(ref iNextCmd);
                    break;

                case BfCommand.Decrement:
                    _handlers.DecrementCell(ref iNextCmd);
                    break;

                case BfCommand.Increment:
                    _handlers.IncrementCell(ref iNextCmd);
                    break;

                case BfCommand.Read:
                    _handlers.ReadIntoCell(ref iNextCmd);
                    break;

                case BfCommand.Print:
                    _handlers.PrintCell(ref iNextCmd);
                    break;

                case BfCommand.OpenLoop:
                    _handlers.OpenLoop(ref iNextCmd);
                    break;

                case BfCommand.CloseLoop:
                    _handlers.CloseLoop(ref iNextCmd);
                    break;
            }
        }
    }
}