using Nintenlord.Distributions.Discrete;
using System;
using System.Linq;

namespace EclipseCombatCalculator.Library.Dices
{
    public sealed class Dice
    {
        public IDiscreteDistribution<DiceFace> FaceDistribution { get; }

        private Dice(DiceFace[] faces)
        {
            int index = 0;
            foreach (var face in faces)
            {
                face.FaceIndex = index;
                index++;
                if (face.Dice != null)
                {
                    throw new ArgumentException("Dice faces cannot be shared among dice", nameof(faces));
                }
                face.Dice = this;
            }
            FaceDistribution = faces.Cast<DiceFace>().ToDistribution();
        }

        public static Dice Create(params DiceFace[] faces)
        {
            return new Dice(faces);
        }

        public static Dice CreateStandard(int damage)
        {
            var faces = new DiceFace[] {
                new() {
                    DamageToOpponent = damage,
                },
                new() {
                    DamageToOpponent = damage,
                    Number = 5,
                },
                new() {
                    DamageToOpponent = damage,
                    Number = 4,
                },
                new() {
                    DamageToOpponent = damage,
                    Number = 3,
                },
                new() {
                    DamageToOpponent = damage,
                    Number = 2,
                },
                new()
            };

            return new Dice(faces);
        }
    }
}
