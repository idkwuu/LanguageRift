using System.ComponentModel;
using System.Reflection;

namespace LanguageRift.Data;

public class LanguageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public static string Version => Assembly.GetEntryAssembly()!.GetName().Version!.ToString();

    private string _gameFolder = "";
    private bool _isGameFolderValid = false;
    private string _language = "";
    private string _region = "";

    public string GameFolder
    {
        get => _gameFolder;
        set
        {
            _gameFolder = value;
            Settings.Default.GameFolder = value;
            Settings.Default.Save();
            OnPropertyChanged(nameof(GameFolder));
        }
    }

    public bool IsGameFolderValid
    {
        get => _isGameFolderValid;
        set
        {
            _isGameFolderValid = value;
            OnPropertyChanged(nameof(IsGameFolderValid));
        }
    }

    public string Language
    {
        get => _language;
        set
        {
            _language = value;
            OnPropertyChanged(nameof(Language));
        }
    }

    public string Region
    {
        get => _region;
        set
        {
            _region = value;
            OnPropertyChanged(nameof(Region));
        }
    }
}