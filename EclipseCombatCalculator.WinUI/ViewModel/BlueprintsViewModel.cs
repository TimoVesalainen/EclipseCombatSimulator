using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class BlueprintsViewModel : INotifyPropertyChanged
    {
        public Blueprint selectedBlueprint;
        public Blueprint SelectedBlueprint
        {
            get { return selectedBlueprint; }
            set
            {
                selectedBlueprint = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
