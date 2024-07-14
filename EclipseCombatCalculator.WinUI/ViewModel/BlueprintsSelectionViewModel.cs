using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class BlueprintsSelectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LayoutListViewModel> Blueprints { get; } = [];

        Blueprint selectedItem;
        public Blueprint SelectedItem 
        { 
            get
            {
                return selectedItem;
            } 
            set
            {
                selectedItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CanSelect));
            }
        }

        public bool CanSelect => SelectedItem != null;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
