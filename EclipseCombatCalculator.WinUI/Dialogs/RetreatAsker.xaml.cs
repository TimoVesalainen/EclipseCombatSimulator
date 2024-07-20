using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.WinUI.ViewModel;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class RetreatAsker : ContentDialog
    {
        public RetreatAskerViewModel ViewModel { get; } = new();

        public RetreatAsker()
        {
            this.InitializeComponent();
        }
    }
}
