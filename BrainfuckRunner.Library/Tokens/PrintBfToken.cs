namespace BrainfuckRunner.Library.Tokens
{
    public sealed class PrintBfToken : BfToken, IBrainfuckCommand
    {
        public override int Length
        {
            get
            {
                return 1;
            }
        }

        public BfCommand Command
        {
            get
            {
                return BfCommand.Print;
            }
        }

        public override string ToString()
        {
            return $"{SysPrefix} (Length: {Length}, Parent: {ParentSysPrefix})";
        }
    }
}