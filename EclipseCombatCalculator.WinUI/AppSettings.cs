using EclipseCombatCalculator.Library.Blueprints;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EclipseCombatCalculator.WinUI
{
    public sealed class AppSettings
    {
        public List<Blueprint> CustomBlueprints { get; set; }
    }

    /// <summary>
    /// Exists to fix IL2026 warning
    /// </summary>
    [JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
    [JsonSerializable(typeof(AppSettings))]
    public partial class AppSettingsContext : JsonSerializerContext
    {

    }
}
