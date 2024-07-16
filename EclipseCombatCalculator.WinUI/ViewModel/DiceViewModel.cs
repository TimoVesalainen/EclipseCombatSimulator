using EclipseCombatCalculator.Library.Dices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class DiceViewModel : INotifyPropertyChanged
    {
        public IDiceFace Dice { get; set; }

        public string Text => TextCreator(Dice);

        private static string TextCreator(IDiceFace diceFace)
        {
            return "Dice";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
