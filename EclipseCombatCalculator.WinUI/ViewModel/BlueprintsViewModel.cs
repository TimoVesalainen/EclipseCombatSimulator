using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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

        private Visibility readOnlyVisibility = Visibility.Collapsed;
        public Visibility ReadOnlyVisibility
        {
            get => readOnlyVisibility;
            set
            {
                readOnlyVisibility = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility writeableVisibility = Visibility.Collapsed;
        public Visibility WriteableVisibility
        {
            get => writeableVisibility;
            set
            {
                writeableVisibility = value;
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

            ReadOnlyVisibility = selectedBlueprint != null && !selectedBlueprint.CanEdit
                ? Visibility.Visible
                : Visibility.Collapsed;

            WriteableVisibility = selectedBlueprint != null && selectedBlueprint.CanEdit
                ? Visibility.Visible
                : Visibility.Collapsed;

            UpdateWarnings();
        }

        private IEnumerable<string> GetWarnings()
        {
            if (selectedBlueprint == null)
            {
                yield break;
            }
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

        public void UpdateWarnings()
        {
            Warnings = string.Join("\n", GetWarnings());
        }
    }
}
