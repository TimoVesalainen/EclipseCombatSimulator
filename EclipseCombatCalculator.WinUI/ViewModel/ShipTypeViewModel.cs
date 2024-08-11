using EclipseCombatCalculator.Library;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class ShipTypeViewModel(string name, IShipTypeStats blueprint) : ViewModel
    {
        private string name = name ?? throw new ArgumentNullException(nameof(name));
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
        public IShipTypeStats Blueprint { get; } = blueprint ?? throw new ArgumentNullException(nameof(blueprint));

        public static ShipTypeViewModel Create(IShipTypeStats blueprint)
        {
            return new ShipTypeViewModel(blueprint.Name, blueprint);
        }
    }
}
