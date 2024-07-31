using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class PartViewModel(Part part, BitmapImage image) : ViewModel
    {
        public Part Part { get; } = part ?? throw new ArgumentNullException(nameof(part));
        public BitmapImage Image { get; } = image ?? throw new ArgumentNullException(nameof(image));

        public static PartViewModel Create(Part part)
        {
            return new PartViewModel(part, part.GetImage());
        }
    }
}
