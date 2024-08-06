using EclipseCombatCalculator.Library.Blueprints;
using System.Collections.ObjectModel;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class BlueprintsSelectionViewModel : ViewModel
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
    }
}
