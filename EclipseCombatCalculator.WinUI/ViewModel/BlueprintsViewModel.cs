using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class BlueprintsViewModel : INotifyPropertyChanged
    {
        private Blueprint selectedBlueprint;
        public Blueprint SelectedBlueprint
        {
            get { return selectedBlueprint; }
            set
            {
                selectedBlueprint = value;
                NotifyPropertyChanged();
                UpdateDerivedProperties();
            }
        }

        private Visibility showBlueprint = Visibility.Collapsed;
        public Visibility ShowBlueprint
        {
            get => showBlueprint;
            set
            {
                showBlueprint = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility readOnlyNameVisibility = Visibility.Collapsed;
        public Visibility ReadOnlyNameVisibility
        {
            get => readOnlyNameVisibility;
            set
            {
                readOnlyNameVisibility = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility writeableNameVisibility = Visibility.Collapsed;
        public Visibility WriteableNameVisibility
        {
            get => writeableNameVisibility;
            set
            {
                writeableNameVisibility = value;
                NotifyPropertyChanged();
            }
        }

        private string warnings = "";
        public string Warnings
        {
            get => warnings;
            set
            {
                warnings = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<LayoutListViewModel> Blueprints { get; } =
            new ObservableCollection<LayoutListViewModel>(Blueprint.Blueprints.Select(LayoutListViewModel.Create));

        private void UpdateDerivedProperties()
        {
            ShowBlueprint = selectedBlueprint != null ? Visibility.Visible : Visibility.Collapsed;

            ReadOnlyNameVisibility = selectedBlueprint != null && !selectedBlueprint.CanEdit
                ? Visibility.Visible
                : Visibility.Collapsed;

            WriteableNameVisibility = selectedBlueprint != null && selectedBlueprint.CanEdit
                ? Visibility.Visible
                : Visibility.Collapsed;

            Warnings = string.Join("\n", GetWarnings());
        }

        private IEnumerable<string> GetWarnings()
        {
            static bool IsDrive(Part part)
            {
                return part != null && part.Movement > 0 || part == Part.JumpDrive;
            }

            if (selectedBlueprint.IsBase)
            {
                if (selectedBlueprint.Parts.Any(IsDrive))
                {
                    yield return "Base isn't supposed to have a drive";
                }
            }
            else
            {
                if (!selectedBlueprint.Parts.Any(IsDrive))
                {
                    yield return "Ship is supposed to have a drive";
                }
            }

            if (selectedBlueprint.TotalEnergy < 0)
            {
                yield return "Not enough energy production";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
