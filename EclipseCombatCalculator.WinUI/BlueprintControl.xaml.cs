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
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.ApplicationModel;
using Nintenlord.Collections;
using EclipseCombatCalculator.Library;
using EclipseCombatCalculator.WinUI.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI
{
    public sealed partial class BlueprintControl : UserControl
    {
        private Blueprint blueprint;

        public Blueprint Blueprint
        {
            get { return blueprint; }
            set
            {
                blueprint = value;
                SetBlueprintParts(value);
                AdjustOffset(value);
                SetTexts(value);
            }
        }

        public bool CanEdit { get; set; } = false;

        public BlueprintControl()
        {
            this.InitializeComponent();
        }

        void SetBlueprintParts(Blueprint blueprint)
        {
            if (blueprint == null)
            {
                foreach (var button in Buttons)
                {
                    button.IsEnabled = false;
                    (button.Content as Image).Source = null;
                }
                return;
            }

            foreach (var (pair, button) in GetPartSlots(blueprint).Zip(Buttons))
            {
                var (isUsed, part) = pair;
                button.IsEnabled = isUsed && blueprint.CanEdit && CanEdit;
                button.Visibility = isUsed ? Visibility.Visible : Visibility.Collapsed;
                (button.Content as Image).Source = part?.GetImage() ?? PartImages.EmptyPart;
            }
        }

        private void AdjustOffset(Blueprint value)
        {
            if (value == null)
            {
                return;
            }

            var (c0, c1, c2, c3) = GetOffsets(value.ShipType, value.Species);
            var offset = 50;

            foreach (var button in Buttons)
            {
                button.Margin = Grid.GetColumn(button) switch
                {
                    0 => new Thickness(0, -offset * c0, 0, offset * c0),
                    1 => new Thickness(0, -offset * c1, 0, offset * c1),
                    2 => new Thickness(0, -offset * c2, 0, offset * c2),
                    3 => new Thickness(0, -offset * c3, 0, offset * c3),
                    _ => throw new IndexOutOfRangeException("Abnormal column in grid image view"),
                };
            }
        }

        private (int, int, int, int) GetOffsets(ShipType shipType, Species species)
        {
            if (species == Species.Exiles && shipType == ShipType.Starbase)
            {
                //Exiles orbital
                // Row 2: Down
                return (0, 2, 0, 0);
            }
            if (species == Species.Planta && shipType == ShipType.Starbase)
            {
                // Row 2: Down
                return (0, 2, 0, 0);
            }

            return shipType switch
            {
                // Row 1: Up
                // Row 3: Up
                ShipType.Interceptor => (1, 0, 1, 0),
                // Row 1: Down
                // Row 3: Down
                ShipType.Cruiser => (-1, 0, -1, 0),
                // Row 1: Down
                // Row 4: Down
                ShipType.Dreadnaught => (-1, 0, 0, -1),
                // Row 2: Up
                ShipType.Starbase => (0, -2, 0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(shipType)),
            };
        }

        private void SetTexts(Blueprint value)
        {
            if (value == null)
            {
                return;
            }
            this.Initiative.Text = $"Initiative: {value.BaseInitiative}";
            this.Computer.Text = $"Computer: {value.BaseComputer}";
            this.Shield.Text = $"Shield: {value.BaseShield}";
            this.Energy.Text = $"Energy: {value.BaseEnergy}";
            this.Hull.Text = $"Hull: {value.BaseHull}";
        }

        static private IEnumerable<(bool isUsed, Part part)> GetPartSlots(Blueprint blueprint)
        {
            if (blueprint.Species == Species.Planta)
            {
                switch (blueprint.ShipType)
                {
                    case ShipType.Interceptor:
                        IEnumerable<(bool isUsed, Part part)> InterceptorParts()
                        {
                            var (p1, p2, p3) = blueprint.Parts.GetFirst3();
                            yield return (false, null);
                            yield return (true, p1);
                            yield return (false, null);
                            yield return (false, null);

                            yield return (true, p2);
                            yield return (false, null);
                            yield return (true, p3);
                            yield return (false, null);
                        }
                        return InterceptorParts();
                    case ShipType.Cruiser:
                        IEnumerable<(bool isUsed, Part part)> CruiserParts()
                        {
                            var (p1, p2, p3, p4, p5) = blueprint.Parts.GetFirst5();
                            yield return (true, p1);
                            yield return (true, p2);
                            yield return (true, p3);
                            yield return (false, null);

                            yield return (false, null);
                            yield return (true, p4);
                            yield return (true, p5);
                            yield return (false, null);
                        }
                        return CruiserParts();
                    case ShipType.Dreadnaught:
                        IEnumerable<(bool isUsed, Part part)> DreadnaughtParts()
                        {
                            var (p1, p2, p3, p4, p5, p6, p7) = blueprint.Parts.GetFirst7();
                            yield return (true, p1);
                            yield return (true, p2);
                            yield return (true, p3);
                            yield return (true, p4);

                            yield return (false, null);
                            yield return (true, p5);
                            yield return (true, p6);
                            yield return (true, p7);
                        }
                        return DreadnaughtParts();
                    case ShipType.Starbase:
                        IEnumerable<(bool isUsed, Part part)> StarBaseParts()
                        {
                            var (p1, p2, p3, p4) = blueprint.Parts.GetFirst4();
                            yield return (true, p1);
                            yield return (false, null);
                            yield return (true, p2);
                            yield return (false, null);

                            yield return (false, null);
                            yield return (true, p3);
                            yield return (true, p4);
                            yield return (false, null);
                        }
                        return StarBaseParts();
                }
            }

            if (blueprint.Species == Species.Exiles && blueprint.ShipType == ShipType.Starbase)
            {
                // Exiles orbital
                IEnumerable<(bool isUsed, Part part)> OrbitalParts()
                {
                    var (p1, p2, p3) = blueprint.Parts.GetFirst3();
                    yield return (false, null);
                    yield return (false, null);
                    yield return (true, p1);
                    yield return (false, null);

                    yield return (true, p2);
                    yield return (true, p3);
                    yield return (false, null);
                    yield return (false, null);
                }
                return OrbitalParts();
            }

            switch (blueprint.ShipType)
            {
                case ShipType.Interceptor:
                    IEnumerable<(bool isUsed, Part part)> InterceptorParts()
                    {
                        var (p1, p2, p3, p4) = blueprint.Parts.GetFirst4();
                        yield return (false, null);
                        yield return (true, p1);
                        yield return (false, null);
                        yield return (false, null);

                        yield return (true, p2);
                        yield return (true, p3);
                        yield return (true, p4);
                        yield return (false, null);
                    }
                    return InterceptorParts();

                case ShipType.Cruiser:
                    return blueprint.Parts
                        .GetPartitions3s()
                        .SelectMany(triple => triple.Enumerate().Select(p => (true, p)).Concat((false, null)));

                case ShipType.Dreadnaught:
                    return blueprint.Parts.Select(part => (true, part));

                case ShipType.Starbase:
                    IEnumerable<(bool isUsed, Part part)> StarbaseParts()
                    {
                        var (p1, p2, p3, p4, p5) = blueprint.Parts.GetFirst5();
                        yield return (true, p1);
                        yield return (true, p2);
                        yield return (true, p3);
                        yield return (false, null);

                        yield return (true, p4);
                        yield return (false, null);
                        yield return (true, p5);
                        yield return (false, null);
                    }
                    return StarbaseParts();
            }

            throw new NotImplementedException($"Not implemented blueprint control for {blueprint}");
        }

        IEnumerable<Button> Buttons => BlueprintGrid.Children.OfType<Button>();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewIndex = 4 * Grid.GetRow(sender as FrameworkElement) + Grid.GetColumn(sender as FrameworkElement);
            int index = -1;
            foreach (var (isUsed, part, viewIndexPart) in GetPartSlots(this.blueprint).Select((pair, i) => (pair.isUsed, pair.part, i)))
            {
                if (isUsed)
                {
                    index++;
                }
                if (viewIndex == viewIndexPart)
                {
                    if (!isUsed)
                    {
                        throw new Exception("Clicked on unused part");
                    }
                    break;
                }
            }

            var dialog = new PartSelectionDialog
            {
                XamlRoot = this.XamlRoot,
                SelectedPart = blueprint[index]
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                blueprint[index] = dialog.SelectedPart;
                SetBlueprintParts(blueprint);
                BlueprintEdited?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler BlueprintEdited;
    }
}
