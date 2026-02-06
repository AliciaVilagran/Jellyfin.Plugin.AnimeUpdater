using MediaBrowser.Model.Plugins;

namespace AnimeUpdater.Configuration
{
    public enum SomeOptions
    {
        FirstOption,
        SecondOption
    }
    public class PluginConfiguration : BasePluginConfiguration
    {
        public bool TrueFalseSetting { get; set; }
        public int AnInteger { get; set; }
        public string AString { get; set; }
        public SomeOptions Options { get; set; }
        public PluginConfiguration()
        {
            Options = SomeOptions.SecondOption;
            TrueFalseSetting = true;
            AnInteger = 2;
            AString = "string";
        }
    }
}