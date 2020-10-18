namespace BrainfuckRunner.Library.Executors
{
    /// <summary>
    /// Executor used by Brainfuck engine
    /// </summary>
    internal abstract class BfExecutor
    {
        internal static BfExecutor CreateInstance(BfEngine engine)
        {
            return engine.IsOptimized 
                ? (BfExecutor) new OptimizedBfExecutor(engine) 
                : new SimpleBfExecutor(engine);
        }

        protected BfExecutor(BfEngine engine)
        {
            Engine = engine;
        }

        protected BfEngine Engine
        {
            get;
        }

        /// <summary>
        /// Allows executor to initialize itself
        /// </summary>
        internal virtual void Initialize()
        {
        }

        internal abstract void RunCommand(BfCommand cmd, ref int iNextCmd);
    }
}
