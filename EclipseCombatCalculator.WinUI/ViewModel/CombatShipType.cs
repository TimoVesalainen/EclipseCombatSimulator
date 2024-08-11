using EclipseCombatCalculator.Library;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatShipType : ViewModel
    {
        private int count = 1;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NotifyPropertyChanged();
            }
        }
        public IShipTypeStats ShipType { get; private set; }
        public string Name => ShipType.Name;

        public static CombatShipType Create(IShipTypeStats shipType)
        {
            return new CombatShipType
            {
                Count = 1,
                ShipType = shipType,
            };
        }
    }
}
