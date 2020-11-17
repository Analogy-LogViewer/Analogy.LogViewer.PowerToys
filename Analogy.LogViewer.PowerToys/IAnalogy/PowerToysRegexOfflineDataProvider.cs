using Analogy.Interfaces;
using Analogy.LogViewer.RegexParser;
using Analogy.LogViewer.RegexParser.IAnalogy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
            RegexParser.Managers.UserSettingsManager.UserSettings.Settings.FileOpenDialogFilters = "Plain log text file (*.txt)|*.txt";
            var regexPattern = new RegexPattern(@"(?<Date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{4})\|(?<Level>\w+)\|(?<Source>.+)\|(?<Text>(?s).*)", "yyyy-MM-dd HH:mm:ss.ffff", "", new List<string> { "*.txt" });
            if (!RegexParser.Managers.UserSettingsManager.UserSettings.Settings.RegexPatterns.Contains(regexPattern))
            {
                RegexParser.Managers.UserSettingsManager.UserSettings.Settings.RegexPatterns.Insert(0, regexPattern);
            }

            return base.InitializeDataProviderAsync(logger);
        }

        public override Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            return base.Process(fileName, token, new RemoveLeadingNewLine(messagesHandler));
        }

        private class RemoveLeadingNewLine : ILogMessageCreatedHandler
        {
            private readonly ILogMessageCreatedHandler _messagesHandler;
            public bool ForceNoFileCaching { get; set; }
            public bool DoNotAddToRecentHistory { get; set; }
            public RemoveLeadingNewLine(ILogMessageCreatedHandler nominalMessagesHandler)
            {
                _messagesHandler = nominalMessagesHandler;
                ForceNoFileCaching = _messagesHandler.ForceNoFileCaching;
                DoNotAddToRecentHistory = _messagesHandler.DoNotAddToRecentHistory;
            }


            public void AppendMessage(AnalogyLogMessage message, string dataSource)
            {
                if (message.Text.StartsWith(Environment.NewLine))
                {
                    //message.Text.
                }
                _messagesHandler.AppendMessage(message, dataSource);
            }

            public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
            {
                foreach (var msg in messages)
                {
                    if (msg.Text.StartsWith(Environment.NewLine))
                    {
                        //message.Text.
                    }
                }

                _messagesHandler.AppendMessages(messages, dataSource);
            }

        }
    }
}
