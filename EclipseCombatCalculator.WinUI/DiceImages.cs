using EclipseCombatCalculator.Library.Dices;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;

namespace EclipseCombatCalculator.WinUI
{
    public static class DiceImages
    {
        private readonly static string[] YellowDiceFaces = [
            "IonCannonHit",
            "IonCannon5",
            "IonCannon4",
            "IonCannon3",
            "IonCannon2",
            "IonCannonMiss",
            ];
        private readonly static string[] OrangeDiceFaces = [
            "PlasmaCannonHit",
            "PlasmaCannon5",
            "PlasmaCannon4",
            "PlasmaCannon3",
            "PlasmaCannon2",
            "PlasmaCannonMiss",
            ];
        private readonly static string[] BlueDiceFaces = [
            "SolitonCannonHit",
            "SolitonCannon5",
            "SolitonCannon4",
            "SolitonCannon3",
            "SolitonCannon2",
            "SolitonCannonMiss",
            ];
        private readonly static string[] RedDiceFaces = [
            "AntimatterCannonHit",
            "AntimatterCannon5",
            "AntimatterCannon4",
            "AntimatterCannon3",
            "AntimatterCannon2",
            "AntimatterCannonMiss",
            ];
        private readonly static string[] PurpleDiceFaces = [
            "RiftCannon3_1",
            "RiftCannon2_0",
            "RiftCannon1_1",
            "RiftCannon0_1",
            "RiftCannonMiss",
            "RiftCannonMiss",
            ];

        private static readonly Dictionary<Dice, BitmapImage[]> images = [];

        private static BitmapImage[] GetImages(Dice dice)
        {
            if (images.TryGetValue(dice, out var image))
            {
                return image;
            }

            string[] faceImageNames;
            if (dice == CommonDices.YellowDice)
            {
                faceImageNames = YellowDiceFaces;
            }
            else if (dice == CommonDices.OrangeDice)
            {
                faceImageNames = OrangeDiceFaces;
            }
            else if (dice == CommonDices.BlueDice)
            {
                faceImageNames = BlueDiceFaces;
            }
            else if (dice == CommonDices.RedDice)
            {
                faceImageNames = RedDiceFaces;
            }
            else if (dice == CommonDices.PurpleDice)
            {
                faceImageNames = PurpleDiceFaces;
            }
            else
            {
                throw new NotImplementedException("No idea what to do with dice");
            }

            var diceFaceImages = faceImageNames.Select(fileName => new BitmapImage(new Uri($"{Package.Current.InstalledPath}/Assets/Dice/{fileName}.png"))).ToArray();

            images[dice] = diceFaceImages;

            return diceFaceImages;
        }

        public static BitmapImage GetBitmap(this DiceFace face)
        {
            var faceBitmaps = GetImages(face.Dice);

            return faceBitmaps[face.FaceIndex];
        }
    }
}
