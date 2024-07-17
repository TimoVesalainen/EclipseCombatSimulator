using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace EclipseCombatCalculator.WinUI
{
    static public class PartImages
    {
        private static readonly Dictionary<Part, BitmapImage> images = [];

        public static BitmapImage GetImage(this Part part)
        {
            if (images.TryGetValue(part, out var image))
            {
                return image;
            }

            var fileName = filenames[part];

            string fullPath = $"{Package.Current.InstalledPath}/Assets/Parts/{fileName}.png";

            var newImage = new BitmapImage(new Uri(fullPath));
            images[part] = newImage;
            return newImage;
        }

        private static readonly Dictionary<Part, string> filenames;

        static PartImages()
        {
            filenames = new() {
                // Weapons
                { Part.IonCannon, "IonCannon" },
                { Part.PlasmaCannon, "PlasmaCannon" },
                { Part.SolitonCannon, "SolitonCannon" },
                { Part.AntimatterCannon, "AntimatterCannon" },
                { Part.RiftCannon, "RiftCannon" },
                { Part.FluxMissile, "FluxMissile" },
                { Part.PlasmaMissile, "PlasmaMissile" },

                // Computers
                { Part.ElectronComputer, "ElectronComputer" },
                { Part.PositronComputer, "PositronComputer" },
                { Part.GluonComputer, "GluonComputer" },

                // Shields
                { Part.GaussShield, "GaussShield" },
                { Part.PhaseShield, "PhaseShield" },
                { Part.AbsorptionShield, "AbsorptionShield" },

                // Hulls
                { Part.Hull, "Hull" },
                { Part.ImprovedHull, "ImprovedHull" },
                { Part.ConifoldField, "ConifoldField" },

                 // Drives
                { Part.NuclearDrive, "NuclearDrive" },
                { Part.FusionDrive, "FusionDrive" },
                { Part.TachyonDrive, "TachyonDrive" },
                { Part.TransitionDrive, "TransitionDrive" },

                // Energy sources
                { Part.NuclearSource, "NuclearSource" },
                { Part.FusionSource, "FusionSource" },
                { Part.TachyonSource, "TachyonSource" },
                { Part.ZeroPointSource, "ZeroPointSource" },

                // Ancient ship parts
                { Part.IonDisruptor, "IonDisruptor" },
                { Part.IonTurret, "IonTurret" },
                { Part.PlasmaTurret, "PlasmaTurret" },
                { Part.SolitonCharger, "SolitonCharger" },
                { Part.RiftConductor, "RiftConductor" },
                { Part.IonMissile, "IonMissile" },
                { Part.SolitonMissile, "SolitonMissile" },
                { Part.AntimatterMissile, "AntimatterMissile" },
                { Part.AxionComputer, "AxionComputer" },
                { Part.FluxShield, "FluxShield" },
                { Part.InversionShield, "InversionShield" },
                { Part.ShardHull, "ShardHull" },
                { Part.ConformalDrive, "ConformalDrive" },
                { Part.NonLinearDrive, "NonLinearDrive" },
                { Part.HypergridSource, "HypergridSource" },

                // Esoteric parts
                { Part.JumpDrive, "JumpDrive" },
                { Part.MorphShield, "MorphShield" },
                { Part.MuonSource, "MuonSource" },
            };
        }
    }
}
