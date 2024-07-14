using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class LayoutListViewModel
    {
        public string Name { get; }
        public Blueprint Blueprint { get; }

        public LayoutListViewModel(string name, Blueprint blueprint)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Blueprint = blueprint ?? throw new ArgumentNullException(nameof(blueprint));
        }

        public static LayoutListViewModel Create(Blueprint blueprint)
        {
            return new LayoutListViewModel(blueprint.Name, blueprint);
        }
    }
}
