using Nintenlord.Distributions.Discrete;

namespace EclipseCombatCalculatorLibrary.Dices
{
    public sealed class Dice
    {
        readonly IDiceFace[] faces;

        public IDiceFace this[int index] => faces[index];

        public IDiscreteDistribution<IDiceFace> FaceDistribution { get; }

        private Dice(IDiceFace[] faces)
        {
            this.faces = faces;
            FaceDistribution = faces.ToDistribution();
        }

        public static Dice Create(params IDiceFace[] faces)
        {
            return new Dice(faces);
        }

        public static Dice CreateStandard(int damage)
        {
            var faces = new IDiceFace[] {
                Damage.Create(damage),
                Number.Create(5, damage),
                Number.Create(4, damage),
                Number.Create(3, damage),
                Number.Create(2, damage),
                Miss.Instance
            };

            return new Dice(faces);
        }
    }
}
