using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class LayoutListViewModel(string name, Blueprint blueprint) : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static LayoutListViewModel Create(Blueprint blueprint)
        {
            return new LayoutListViewModel(blueprint.Name, blueprint);
        }
    }
}
