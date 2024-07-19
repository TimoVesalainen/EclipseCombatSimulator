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
                SetTexts(value);
            }
        }

        public int Damage { get; set; }
        public int Count { get; set; }

        public BlueprintControl()
        {
            this.InitializeComponent();
        }

        void SetBlueprintParts(Blueprint blueprint)
        {
            if (blueprint == null)
            {
                foreach (var view in Images)
                {
                    view.Source = null;
                }
                return;
            }

            foreach (var (pair, view) in GetPartSlots(blueprint).Zip(Images))
            {
                var (isUsed, part) = pair;
                view.Source = part?.GetImage() ?? PartImages.EmptyPart;
                view.Visibility = isUsed ? Visibility.Visible : Visibility.Collapsed;
            }
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

        IEnumerable<Image> Images => BlueprintGrid.Children.OfType<Image>();
    }
}
