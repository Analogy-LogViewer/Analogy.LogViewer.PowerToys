using Analogy.LogViewer.RegexParser;
using Analogy.LogViewer.RegexParser.IAnalogy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class PowerToysRegexOfflineDataProvider : RegexOfflineDataProvider
    {
        public override string OptionalTitle { get; set; } = "PowerToys Regex offline logs";
        public override Guid Id { get; set; } = new Guid("4daa52fe-adcf-4a98-be47-090ba0e77b5f");
        public override string InitialFolderFullPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\PowerToys\PowerToys Run\Logs");
        public override Image? LargeImage { get; set; } = null;
        public override Image? SmallImage { get; set; } = null;

        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            var regexPattern = new RegexPattern(@"(?<Date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{4})\|(?<Level>\w+)\|(?<Source>.+)\|(?<Text>.*)", "yyyy-MM-dd HH:mm:ss.ffff", "", new List<string> { "*.txt" });
            if (!RegexParser.Managers.UserSettingsManager.UserSettings.Settings.RegexPatterns.Contains(regexPattern))
            {
                RegexParser.Managers.UserSettingsManager.UserSettings.Settings.RegexPatterns.Insert(0, regexPattern);
            }

            return base.InitializeDataProviderAsync(logger);
        }
    }
}
