using System.ComponentModel;

namespace LoL_Language
{
    public class DataViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _lolFolder = Properties.Settings.Default.LolFolder;
        private string _language = Properties.Settings.Default.Language;
        private string _region = Properties.Settings.Default.Region;
        private bool _isValidLolFolder = false;
        private int _selectedRegionIndex;
        private int _selectedLanguageIndex;

        public int SelectedRegionIndex
        {
            get => _selectedRegionIndex;
            set
            {
                _selectedRegionIndex = value;
                OnPropertyChanged("SelectedRegionIndex");
            }
        }

        public int SelectedLanguageIndex
        {
            get => _selectedLanguageIndex;
            set
            {
                _selectedLanguageIndex = value;
                OnPropertyChanged("SelectedLanguageIndex");
            }
        }

        public string LolFolder
        {
            get => _lolFolder;
            set
            {
                _lolFolder = value;
                Properties.Settings.Default.LolFolder = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("LolFolder");
            }
        }
        
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                Properties.Settings.Default.Language = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Language");
            }
        }
        
        public string Region
        {
            get => _region;
            set
            {
                _region = value;
                Properties.Settings.Default.Region = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("Region");
            }
        }

        public bool IsValidLolFolder
        {
            get => _isValidLolFolder;
            set
            {
                _isValidLolFolder = value;
                OnPropertyChanged("IsValidLolFolder");
            }
        }
    }
}