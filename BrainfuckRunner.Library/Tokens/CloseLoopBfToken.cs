namespace BrainfuckRunner.Library.Tokens
{
    public sealed class CloseLoopBfToken : BfToken, IBrainfuckCommand
    {
        public override int Length
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets parent token - the one that opens/starts loop
        /// </summary>
        public new LoopBfToken Parent
        {
            get => base.Parent as LoopBfToken;
            internal set => base.Parent = value;
        }

        public BfCommand Command
        {
            get
            {
                return BfCommand.CloseLoop;
            }
        }

        public override string ToString()
        {
            return $"{SysPrefix} (Length: {Length}, Parent: {ParentSysPrefix})";
        }
    }
}