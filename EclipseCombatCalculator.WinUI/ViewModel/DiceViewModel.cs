using EclipseCombatCalculator.Library.Dices;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceViewModel : ViewModel
    {
        public Guid ID { get; } = Guid.NewGuid();

        public DiceFace Dice { get; set; }

        public bool CanHit { get; set; } = true;

        public BitmapImage Image => Dice.GetBitmap();

        public double Opacity => CanHit ? 1.0 : 0.3;
    }
}
