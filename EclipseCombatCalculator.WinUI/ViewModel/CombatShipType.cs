﻿using EclipseCombatCalculator.Library.Blueprints;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EclipseCombatCalculator.WinUI.ViewModel
{
    public sealed class CombatShipType : INotifyPropertyChanged
    {
        private int count = 1;
        public int Count {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NotifyPropertyChanged();
            }
        }
        public Blueprint Blueprint { get; private set; }
        public string Name => Blueprint.Name;

        public event PropertyChangedEventHandler PropertyChanged;

        public static CombatShipType Create(Blueprint blueprint)
        {
            return new CombatShipType
            {
                Count = 1,
                Blueprint = blueprint,
            };
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
