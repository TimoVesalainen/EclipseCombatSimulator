using EclipseCombatCalculator.Library.Blueprints;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class LayoutListViewModel(string name, Blueprint blueprint) : ViewModel
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
        public Blueprint Blueprint { get; } = blueprint ?? throw new ArgumentNullException(nameof(blueprint));

        public static LayoutListViewModel Create(Blueprint blueprint)
        {
            return new LayoutListViewModel(blueprint.Name, blueprint);
        }
    }
}
