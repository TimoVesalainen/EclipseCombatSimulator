using Nintenlord.Distributions.Discrete;

namespace EclipseCombatCalculatorLibrary
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

    }
}
