using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class PartViewModel
    {
        public Part Part { get; }
        public BitmapImage Image { get; }

        private PartViewModel(Part part, BitmapImage image)
        {
            Part = part ?? throw new ArgumentNullException(nameof(part));
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public static PartViewModel Create(Part part)
        {
            return new PartViewModel(part, part.GetImage());
        }
    }
}
