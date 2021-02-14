namespace BrainfuckRunner.Library.Behaviors
{
    /// <summary>
    /// Class wrapper around non-reference structure types
    /// </summary>
    /// <typeparam name="TStruct">Type of structure type</typeparam>
    internal sealed class StructRef<TStruct> where TStruct : struct
    {
        internal StructRef(TStruct valueOnInit)
        {
            Value = valueOnInit;
        }

        internal TStruct Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}