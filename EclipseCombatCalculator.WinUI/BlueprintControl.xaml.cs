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
                var parts = Enumerable.Range(0, value.Size)
                    .Select(index => value[index]);

                foreach (var (part, view) in parts.Zip(Images))
                {
                    view.Source = part?.GetImage();
                }
            }
        }

        public int Damage { get; set; }
        public int Count { get; set; }

        public BlueprintControl()
        {
            this.InitializeComponent();
        }

        public IEnumerable<Image> Images => BlueprintGrid.Children.OfType<Image>();


        private static BitmapImage empty = null;
        public static BitmapImage EmptyImage()
        {
            if (empty != null)
            {
                return empty;
            }
            string fullPath = $"{Package.Current.InstalledPath}/Assets/Parts/EmptyPart.png";

            var newImage = new BitmapImage(new Uri(fullPath));
            empty = newImage;
            return newImage;
        }
    }
}
