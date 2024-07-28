﻿using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EclipseCombatCalculator.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private const string SaveFileName = "customLayouts.json";
        public List<Blueprint> CustomBlueprints { get; } = [];
        readonly JsonSerializerOptions options = new();
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            options.Converters.Add(new JsonStringEnumConverter());
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var path = Package.Current.InstalledPath + "/" + SaveFileName;
            if (File.Exists(path))
            {
                using var stream = File.OpenRead(path);
                var tempValue = JsonSerializer.Deserialize<AppSettings>(stream, options);

                foreach (var blueprint in tempValue.CustomBlueprints)
                {
                    CustomBlueprints.Add(blueprint);
                }
            }

            m_window = new MainWindow();
            m_window.Closed += Window_Closed;
            m_window.Activate();
        }

        private async void Window_Closed(object sender, WindowEventArgs args)
        {
            var path = Package.Current.InstalledPath + "/" + SaveFileName;
            using var stream = File.Open(path, FileMode.Create);
            using var textWriter = new StreamWriter(stream);

            string jsonString = JsonSerializer.Serialize(new AppSettings { CustomBlueprints = CustomBlueprints }, options);
            await textWriter.WriteAsync(jsonString);
            await textWriter.FlushAsync();
        }

        private Window m_window;
    }
}
