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
                SetBLueprintParts(value);
            }
        }

        public int Damage { get; set; }
        public int Count { get; set; }

        public BlueprintControl()
        {
            this.InitializeComponent();
        }

        void SetBLueprintParts(Blueprint blueprint)
        {
            if (blueprint == null)
            {
                foreach (var view in Images)
                {
                    view.Source = null;
                }
                return;
            }

            foreach (var (part, view) in GetPartSlots(blueprint).Zip(Images))
            {
                view.Source = part?.GetImage();
            }
        }

        static private IEnumerable<Part> GetPartSlots(Blueprint blueprint)
        {
            if (blueprint.Species == Species.Planta)
            {
                switch (blueprint.ShipType)
                {
                    case ShipType.Interceptor:
                        IEnumerable<Part> InterceptorParts()
                        {
                            var (p1, p2, p3) = blueprint.Parts.GetFirst3();
                            yield return null;
                            yield return p1;
                            yield return null;
                            yield return null;

                            yield return p2;
                            yield return null;
                            yield return p3;
                            yield return null;
                        }
                        return InterceptorParts();
                    case ShipType.Cruiser:
                        IEnumerable<Part> CruiserParts()
                        {
                            var (p1, p2, p3, p4, p5) = blueprint.Parts.GetFirst5();
                            yield return p1;
                            yield return p2;
                            yield return p3;
                            yield return null;

                            yield return null;
                            yield return p4;
                            yield return p5;
                            yield return null;
                        }
                        return CruiserParts();
                    case ShipType.Dreadnaught:
                        IEnumerable<Part> DreadnaughtParts()
                        {
                            var (p1, p2, p3, p4, p5, p6, p7) = blueprint.Parts.GetFirst7();
                            yield return p1;
                            yield return p2;
                            yield return p3;
                            yield return p4;

                            yield return null;
                            yield return p5;
                            yield return p6;
                            yield return p7;
                        }
                        return DreadnaughtParts();
                    case ShipType.Starbase:
                        IEnumerable<Part> StarBaseParts()
                        {
                            var (p1, p2, p3, p4, p5) = blueprint.Parts.GetFirst5();
                            yield return p1;
                            yield return null;
                            yield return p2;
                            yield return null;

                            yield return null;
                            yield return p3;
                            yield return p4;
                            yield return null;
                        }
                        return StarBaseParts();
                }
            }

            if (blueprint.Species == Species.Exiles && blueprint.ShipType == ShipType.Starbase)
            {
                // Exiles orbital
                IEnumerable<Part> OrbitalParts()
                {
                    var (p1, p2, p3) = blueprint.Parts.GetFirst3();
                    yield return null;
                    yield return null;
                    yield return p1;

                    yield return p2;
                    yield return p3;
                    yield return null;
                }
                return OrbitalParts();
            }

            switch (blueprint.ShipType)
            {
                case ShipType.Interceptor:
                    IEnumerable<Part> InterceptorParts()
                    {
                        var (p1, p2, p3, p4) = blueprint.Parts.GetFirst4();
                        yield return null;
                        yield return p1;
                        yield return null;
                        yield return null;

                        yield return p2;
                        yield return p3;
                        yield return p4;
                        yield return null;
                    }
                    return InterceptorParts();

                case ShipType.Cruiser:
                    return blueprint.Parts
                        .GetPartitions3s()
                        .SelectMany(triple => triple.Enumerate().Concat(default(Part)));

                case ShipType.Dreadnaught:
                    return blueprint.Parts;

                case ShipType.Starbase:
                    IEnumerable<Part> StarbaseParts()
                    {
                        var (p1, p2, p3, p4, p5) = blueprint.Parts.GetFirst5();
                        yield return p1;
                        yield return p2;
                        yield return p3;
                        yield return null;

                        yield return p4;
                        yield return null;
                        yield return p5;
                        yield return null;
                    }
                    return StarbaseParts();
            }

            throw new NotImplementedException($"Not implemented blueprint control for {blueprint}");
        }

        public IEnumerable<Image> Images => BlueprintGrid.Children.OfType<Image>();
    }
}
