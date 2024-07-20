using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class PartSelectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PartViewModel> Parts = new(Part.AllPArts.Select(PartViewModel.Create));

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
