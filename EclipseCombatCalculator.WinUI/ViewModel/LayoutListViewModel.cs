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
        public LayoutListViewModel()
        {

        }

        public string Name { get; init; }

        public static LayoutListViewModel Create(Blueprint blueprint)
        {
            return new LayoutListViewModel() { Name = blueprint.Name };
        }
    }
}
