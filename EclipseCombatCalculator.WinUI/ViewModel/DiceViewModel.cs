using EclipseCombatCalculator.Library.Dices;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceViewModel : ViewModel
    {
        public Guid ID { get; } = Guid.NewGuid();

        public DiceFace Dice
        {
            get => dice;
            set
            {
                if (dice == value) return;
                dice = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanHit
        {
            get => canHit;
            set
            {
                if (canHit == value) return;
                canHit = value;
                NotifyPropertyChanged();
            }
        }

        public BitmapImage Image => Dice.GetBitmap();

        public double Opacity => CanHit ? 1.0 : 0.3;

        private bool canHit = true;
        private DiceFace dice;
    }
}
