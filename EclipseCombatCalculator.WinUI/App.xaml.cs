using EclipseCombatCalculator.Library.Blueprints;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            m_window = new MainWindow();
            m_window.Closed += Window_Closed;
            m_window.Activate();
        }

        private async void Window_Closed(object sender, WindowEventArgs args)
        {
            var path = Package.Current.InstalledPath + "/" + SaveFileName;
            using var stream = File.OpenWrite(path);
            using var textWriter = new StreamWriter(stream);

            string jsonString = JsonSerializer.Serialize(new { CustomBlueprints }, options);
            await textWriter.WriteAsync(jsonString);
            await textWriter.FlushAsync();
            //TODO: Save custom blueprints
        }

        private Window m_window;
    }
}
