using LanguageRift.Data;
using LanguageRift.Helpers;
using Ookii.Dialogs.Wpf;
using System;
using System.Diagnostics;
using System.Windows;

namespace LanguageRift.Views
{
    public partial class MainWindow : Window
    {
        private readonly LanguageViewModel _lvm = new();
        private readonly GameFolderHelper _gameFolderHelper = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _lvm;
            // Start doing things
            DoGameFolderThings(_gameFolderHelper.FindFolder());
            // Make buttons go brrr
            Licenses.Click += LaunchLicenses;
            GitHub.Click += LaunchGitHub;
            Launch.Click += LaunchGame;
            OpenFolder.Click += OpenFolderDialog;

        }

        private void DoGameFolderThings(string? gameFolder)
        {
            if (gameFolder != null)
            {
                _lvm.IsGameFolderValid = true;
                _lvm.GameFolder = gameFolder;
                GameData data = _gameFolderHelper.GetCurrentSettings(gameFolder);
                _lvm.Region = data.Region;
                _lvm.Language = data.Language;
            }
            else
            {
                _lvm.IsGameFolderValid = false;
                _lvm.GameFolder = "";
            }
        }

        private void OpenFolderDialog(object sender, dynamic args)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Please select the League of Legends folder.",
                UseDescriptionForTitle = true
            };
            if ((bool)dialog.ShowDialog(this)!)
            {
                var path = _gameFolderHelper.FindFolder(dialog.SelectedPath);
                DoGameFolderThings(_gameFolderHelper.FindFolder(path));
                if (path == null)
                {
                    MessageBox.Show("Selected folder didn't contain the League of Legends client.");
                }
            }
        }

        private void LaunchGitHub(object sender, dynamic args)
        {
            Process.Start(new ProcessStartInfo("https://github.com/idkwuu/LanguageRift") { UseShellExecute = true });
        }

        private void LaunchLicenses(object sender, dynamic args)
        {
            Licenses licensesWindow = new()
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            licensesWindow.ShowDialog();
        }

        private void LaunchGame(object sender, dynamic args)
        {
            _gameFolderHelper.SaveSettings(_lvm.GameFolder, _lvm.Language, _lvm.Region);
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = _lvm.GameFolder + "\\LeagueClient.exe",
                Arguments = "--locale=" + _lvm.Language
            };
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
