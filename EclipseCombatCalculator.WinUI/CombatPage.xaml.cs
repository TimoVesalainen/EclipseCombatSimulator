using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EclipseCombatCalculator.Library.Blueprints;
using EclipseCombatCalculator.WinUI.ViewModel;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CombatPage : Page
    {
        public ObservableCollection<CombatShipType> Attackers { get; } = [CombatShipType.Create(Blueprint.TerranInterceptor)];
        public ObservableCollection<CombatShipType> Defenders { get; } = [CombatShipType.Create(Blueprint.OrionCruiser)];

        public CombatPage()
        {
            this.InitializeComponent();
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            viewModel.Count += 1;
            Attackers.Add(CombatShipType.Create(Blueprint.TerranDreadnaught));
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (e.OriginalSource as Button).DataContext as CombatShipType;
            viewModel.Count -= 1;
        }
    }
}
