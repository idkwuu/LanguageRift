namespace LanguageRift.Data
{
    public class GameData
    {
        public string Language { get; }
        public string Region { get; }

        public GameData(string language, string region)
        {
            Language = language;
            Region = region;
        }
    }
}
