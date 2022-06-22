using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using LanguageRift.Data;

namespace LanguageRift.Helpers
{
    internal class GameFolderHelper
    {
        private Dictionary<string, object> _clientSettings;
        private Dictionary<object, object> _clientSettingsInstall;
        private Dictionary<object, object> _clientSettingsInstallGlobals;
        private const string _clientSettingsFilePath = "\\Config\\LeagueClientSettings.yaml";

        public string? FindFolder(string selectedFolder = null)
        {
            var paths = new List<string>{
                selectedFolder,
                Settings.Default.GameFolder,
                "C:\\Riot Games\\League of Legends"
            };

            for (var i = 0; i < paths.Count; i++)
            {
                var fileExist = File.Exists($"{paths[i]}\\LeagueClient.exe");
                if (selectedFolder != null && !fileExist) return null; // If a path was set by the user, then return null to actually show the error messag.
                if (!fileExist) continue; // Other paths couldn't be found
                return paths[i];
            }
            return null;
        }

        /// <summary>
        /// Find the current region and language set for the game.
        /// </summary>
        public GameData GetCurrentSettings(string gameFolder)
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            string clientSettingsPath = $"{gameFolder}{_clientSettingsFilePath}";
            TextReader clientSettingsFile = new StreamReader(clientSettingsPath);

            _clientSettings = deserializer.Deserialize<Dictionary<string, object>>(clientSettingsFile);
            clientSettingsFile.Close(); // Close after reading.
            _clientSettingsInstall = (Dictionary<object, object>)_clientSettings["install"];
            _clientSettingsInstallGlobals = (Dictionary<object, object>)_clientSettingsInstall["globals"];
            var language = (string)_clientSettingsInstallGlobals["locale"];
            var region = (string)_clientSettingsInstallGlobals["region"];

            return new GameData(language, region);
        }

        public void SaveSettings(string gameFolder, string language, string region)
        {
            // Read system settings
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance) // For whatever reason, Riot uses a different naming convention for this file... wtf?
                .Build();

            string systemSettingsPath = $"{gameFolder}\\system.yaml";
            TextReader systemSettingsFile = new StreamReader(systemSettingsPath);
            var systemSettings = deserializer.Deserialize<Dictionary<string, object>>(systemSettingsFile);
            systemSettingsFile.Close(); // Close after reading.
            var regionData = (Dictionary<object, object>)systemSettings["region_data"];
            var yourRegion = (Dictionary<object, object>)regionData[region];

            // Create new language data for system settings
            var newAvailableLocales = new List<string>
            {
                language
            };

            yourRegion["available_locales"] = newAvailableLocales;
            yourRegion["default_locale"] = language;

            regionData[region] = yourRegion;
            systemSettings["region_data"] = regionData;

            // Save system settings
            ISerializer serializerUnderscored = new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var systemSettingsData = serializerUnderscored.Serialize(systemSettings);
            File.WriteAllText(systemSettingsPath, systemSettingsData);

            // Edit language data in client settings
            _clientSettingsInstallGlobals["locale"] = language;
            _clientSettingsInstall["globals"] = _clientSettingsInstallGlobals;
            _clientSettings["install"] = _clientSettingsInstall;

            // Write client settings
            ISerializer serializerHyphenated = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            var clientSettingsData = serializerHyphenated.Serialize(_clientSettings);
            File.WriteAllText($"{gameFolder}{_clientSettingsFilePath}", clientSettingsData);
        }
    }
}
