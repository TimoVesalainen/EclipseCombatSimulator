using EclipseCombatCalculator.Library.Dices;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceViewModel : INotifyPropertyChanged
    {
        public Guid ID { get; } = Guid.NewGuid();

        public DiceFace Dice { get; set; }

        public BitmapImage Image => Dice.GetBitmap();

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
