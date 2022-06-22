using System.Diagnostics;
using System.Windows;

namespace LanguageRift.Views
{
    public partial class Licenses : Window
    {
        public Licenses()
        {
            InitializeComponent();
            ModernWPF.Click += OpenModernWPF;
            YamlDotNet.Click += OpenYamlDotNet;
            Ookii.Click += OpenOokii;
        }

        private void OpenModernWPF(object sender, dynamic args)
        {
            OpenLink("https://github.com/Kinnara/ModernWpf");
        }

        private void OpenYamlDotNet(object sender, dynamic args)
        {
            OpenLink("https://github.com/aaubry/YamlDotNet/");
        }

        private void OpenOokii(object sender, dynamic args)
        {
            OpenLink("https://github.com/ookii-dialogs/ookii-dialogs-wpf/");
        }

        private void OpenLink(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
