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

            foreach (var (part, view) in GetPartSlots().Zip(Images))
            {
                view.Source = part?.GetImage();
            }
        }

        IEnumerable<Part> GetPartSlots()
        {
            return Enumerable.Range(0, blueprint.Size)
                .Select(index => blueprint[index])
                .ConcatInfinite(null);
        }

        public IEnumerable<Image> Images => BlueprintGrid.Children.OfType<Image>();
    }
}
