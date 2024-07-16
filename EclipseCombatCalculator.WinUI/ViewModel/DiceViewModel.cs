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
        public Guid ID { get; } = Guid.NewGuid();

        public IDiceFace Dice { get; set; }

        public string Text => TextCreator(Dice);

        private static string TextCreator(IDiceFace diceFace)
        {
            return diceFace switch
            {
                Damage damage => string.Join("", Enumerable.Range(0, damage.DamageToOpponent).Select(_ => "*")),
                Number number => $"{number.Value}({string.Join("", Enumerable.Range(0, number.DamageToOpponent).Select(_ => "*"))})",
                Miss number => "_",
                _ => throw new NotImplementedException(),
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
