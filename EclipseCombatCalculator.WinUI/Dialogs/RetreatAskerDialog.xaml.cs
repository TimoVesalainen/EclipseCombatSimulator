using Microsoft.UI.Xaml.Controls;
using EclipseCombatCalculator.WinUI.ViewModel;

namespace EclipseCombatCalculator.WinUI.Dialogs
{
    public sealed partial class RetreatAskerDialog : ContentDialog
    {
        public RetreatAskerViewModel ViewModel { get; } = new();

        public RetreatAskerDialog()
        {
            this.InitializeComponent();
        }
    }
}
