using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AnimeUpdater.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace AnimeUpdater
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly IUserDataManager _userDataManager;
    private readonly IUserManager _userManager;
    private readonly string _logFile;
    public override string Name => "Anime Updater";
    public override Guid Id => Guid.Parse("98f6a3be-3289-45fa-9215-5bafb46a7d6a");
    public static Plugin? Instance { get; private set; }
    
    public Plugin(
        IApplicationPaths applicationPaths,
        IUserDataManager userDataManager,
        IUserManager userManager,
        IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
        _userManager = userManager;
        _userDataManager = userDataManager;
        _logFile = Path.Combine(
            applicationPaths.LogDirectoryPath,
            "watched-episodes.log"
        );

        File.AppendAllText(
            Path.Combine(applicationPaths.LogDirectoryPath, "watched-BOOT.log"),
            $"PLUGIN LOADED {DateTime.Now}\n"
        );
        
        _userDataManager.UserDataSaved += OnUserDataSaved;
    }
    
    private void OnUserDataSaved(object sender, UserDataSaveEventArgs e)
    {
        if (e.Item is not Episode episode)
            return;
        
        if (!e.UserData.Played)
            return;

        var seriesName = episode.SeriesName;
        var season = episode.ParentIndexNumber ?? 0;
        var episodeNum = episode.IndexNumber ?? 0;
        var user = _userManager.GetUserById(e.UserId)?.Username ?? "Unknown";

        var line =
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
            $"{user} | {seriesName} S{season:D2}E{episodeNum:D2}";

        File.AppendAllText(_logFile, line + Environment.NewLine);
    }
    
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.configPage.html", GetType().Namespace)
            }
        ];
    }
}
}