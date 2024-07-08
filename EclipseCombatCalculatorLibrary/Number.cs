namespace EclipseCombatCalculatorLibrary
{
    public sealed class Number : IDiceFace 
    {
        public int Value { get; }

        private Number(int value)
        {
            Value = value;
        }

        public static readonly Number Two = new Number(2);
        public static readonly Number Three = new Number(3);
        public static readonly Number Four = new Number(4);
        public static readonly Number Five = new Number(5);
    }
}
