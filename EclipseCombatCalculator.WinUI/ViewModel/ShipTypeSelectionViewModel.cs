using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.Library.Blueprints;
using System.Collections.ObjectModel;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class ShipTypeSelectionViewModel : ViewModel
    {
        public ObservableCollection<ShipTypeViewModel> ShipTypes { get; } = [];

        IShipTypeStats selectedItem;
        public IShipTypeStats SelectedItem
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
