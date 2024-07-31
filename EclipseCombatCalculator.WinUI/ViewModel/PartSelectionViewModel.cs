using EclipseCombatCalculator.Library.Blueprints;
using System.Collections.ObjectModel;
using System.Linq;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class PartSelectionViewModel : ViewModel
    {
        public ObservableCollection<PartViewModel> Parts { get; } = new(Part.AllPArts.Select(PartViewModel.Create));
    }
}
