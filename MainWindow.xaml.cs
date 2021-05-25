using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace LoL_Language
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Variables
        private readonly DataViewModel _dvm = new();
        private string[] _clientSettings;
        private int _clientSettingsLanguageLineNumber;
        private string[] _system;
        private int _systemLanguageLineNumber;
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _dvm;
            // Do things...
            LoL_LookUpFolder();
            LoL_ReadConfigs();
            LoL_GetCurrentLanguageAndRegion();
            // Link buttons to functions
            OpenFolder.Click += OpenFolderPicker;
            Launch.Click += LoL_Launch;
            RegionComboBox.SelectionChanged += RegionSelectionChanged;
            LanguageComboBox.SelectionChanged += LanguageSelectionChanged;
            Licenses.Click += LaunchLicenses;
            GitHub.Click += LaunchGitHub;
        }

        private void LaunchLicenses(object sender, dynamic args)
        {
            Process.Start("https://github.com/idkwuu/LoL-Language/blob/main/acknowledgements.md");
        }
        
        private void LaunchGitHub(object sender, dynamic args)
        {
            Process.Start("https://github.com/idkwuu/LoL-Language");
        }

        private void RegionSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _dvm.Region = args.AddedItems[0].ToString();
            LoL_SetRegion();
            FindIndexOfRegion(_dvm.Region);
        }

        private void FindIndexOfRegion(string data)
        {
            var regions = (string[]) Resources["Regions"];
            _dvm.SelectedRegionIndex = regions.ToList().IndexOf(data);
        }
        
        private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _dvm.Language = args.AddedItems[0].ToString();
            LoL_SetLanguage();
            FindIndexOfLanguage(_dvm.Language);
        }

        private void FindIndexOfLanguage(string data)
        {
            var langs = (string[]) Resources["Languages"];
            _dvm.SelectedLanguageIndex = langs.ToList().IndexOf(data);
        }

        private void OpenFolderPicker(object sender, dynamic args)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Please select the League of Legends folder.", UseDescriptionForTitle = true
            };
            if ((bool) dialog.ShowDialog(this)!)
            {
                var path = dialog.SelectedPath;
                if (!LoL_LookUpFolder(path))
                {
                    MessageBox.Show("Selected folder didn't contain the League of Legends client.");
                }
            }
        }


        #region LoL Related Functions
        private string LoL_GetClientSettingsPath() => _dvm.LolFolder + "\\Config\\LeagueClientSettings.yaml";

        private string LoL_GetSystemYamlPath() => _dvm.LolFolder + "\\system.yaml";
        private void LoL_ReadConfigs()
        {
            _clientSettings = File.ReadAllLines(LoL_GetClientSettingsPath());
            _system = File.ReadAllLines(LoL_GetSystemYamlPath());
        }
        
        private bool LoL_LookUpFolder(string folder = null)
        {
            if (folder != null)
            {
                var path = folder + "\\LeagueClient.exe";
                if (!File.Exists(path))
                {
                    _dvm.IsValidLolFolder = false;
                    return false;
                }
                _dvm.LolFolder = folder;
                _dvm.IsValidLolFolder = true;
                return true;

            }
            // Check if saved folder is valid
            if (File.Exists(_dvm.LolFolder + "\\LeagueClient.exe"))
            {
                // No need to do anything
            }
            // Does it exist in the default path?
            else if (File.Exists("C:\\Riot Games\\League of Legends\\LeagueClient.exe"))
            {
                _dvm.LolFolder = "C:\\Riot Games\\League of Legends"; 
            }
            // No folder found, user has to find it
            else
            {
                _dvm.IsValidLolFolder = false;
                return false;
            }
            _dvm.IsValidLolFolder = true;
            return true;
        }

        private void LoL_GetCurrentLanguageAndRegion()
        {
            for (var i = 0; i < _clientSettings.Length; i++)
            {
                if (_clientSettings[i] != "    globals:") continue;
                _clientSettingsLanguageLineNumber = i;
                break;
            }
            _dvm.Language = _dvm.Language == "" ? _clientSettings[_clientSettingsLanguageLineNumber + 1].Split('\"')[1] : _dvm.Language;
            _dvm.Region = _dvm.Region ==
                         "" ? _clientSettings[_clientSettingsLanguageLineNumber + 2].Split('\"')[1] : _dvm.Region;
            
            FindIndexOfRegion(_dvm.Region);
            FindIndexOfLanguage(_dvm.Language);
            
            for (var i = 0; i < _system.Length; i++)
            {
                if (_system[i] == "  " + _dvm.Region + ":")
                {
                    _systemLanguageLineNumber = i;
                    break;
                }
            }
        }
        
        private void LoL_SetLanguage()
        {
            var clientSettingsLang = _clientSettings[_clientSettingsLanguageLineNumber + 1].Split('\"');
            clientSettingsLang[1] = _dvm.Language;
            _clientSettings[_clientSettingsLanguageLineNumber + 1] = string.Join("\"", clientSettingsLang);

            var systemLang = _system[_systemLanguageLineNumber + 2].Split(' ');
            systemLang[systemLang.Length - 1] = _dvm.Language;
            _system[_systemLanguageLineNumber + 2] = string.Join(" ", systemLang);

            var systemLocaleLang = _system[_systemLanguageLineNumber + 3].Split(' ');
            systemLocaleLang[systemLocaleLang.Length - 1] = _dvm.Language;
            _system[_systemLanguageLineNumber + 3] = string.Join(" ", systemLocaleLang);
        }

        private void LoL_SetRegion()
        {
            LoL_GetCurrentLanguageAndRegion();
            LoL_SetLanguage();
        }

        private void LoL_WriteSettings()
        {
            File.WriteAllLines(LoL_GetClientSettingsPath(), _clientSettings);
            File.WriteAllLines(LoL_GetSystemYamlPath(), _system);
        }
        
        private void LoL_Launch(object sender, dynamic args)
        {
            LoL_WriteSettings();
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = _dvm.LolFolder + "\\LeagueClient.exe",
                Arguments = "--locale=" + _dvm.Language
            };
            Process.Start(startInfo);
            Environment.Exit(0);
        }
        #endregion
    }
}