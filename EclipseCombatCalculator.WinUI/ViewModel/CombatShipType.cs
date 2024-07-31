using EclipseCombatCalculator.Library.Blueprints;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatShipType : ViewModel
    {
        private int count = 1;
        public int Count {
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
        public Blueprint Blueprint { get; private set; }
        public string Name => Blueprint.Name;

        public static CombatShipType Create(Blueprint blueprint)
        {
            return new CombatShipType
            {
                Count = 1,
                Blueprint = blueprint,
            };
        }
    }
}
