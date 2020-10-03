using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.PowerToys.Parser;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class OfflineDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override Image SmallImage { get; set; } = null;
        public override Image LargeImage { get; set; } = null;
        public override string OptionalTitle { get; set; } = "PowerToys Parser";
        public override string FileOpenDialogFilters { get; set; } = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        public override IEnumerable<string> SupportFormats { get; set; } = new List<string>{"*.txt"};
        public override string InitialFolderFullPath { get; } =Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\PowerToys\PowerToys Run\Logs");
        public override Guid Id { get; set; } = new Guid("e8bfbefe-912f-41d2-9faf-2f370d3695bc");
        private PlainTextLogFileLoader parser;
        private SplitterLogParserSettings LogParserSettings { get; set; }
        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
           LogParserSettings = new SplitterLogParserSettings();
            LogParserSettings.Splitter = "|";
            LogParserSettings.SupportedFilesExtensions = new List<string> { "*.txt" };
            LogParserSettings.Maps = new Dictionary<int, AnalogyLogMessagePropertyName>
            {
                {0, AnalogyLogMessagePropertyName.Date}, {1, AnalogyLogMessagePropertyName.Level},
                {2, AnalogyLogMessagePropertyName.Source}, {3, AnalogyLogMessagePropertyName.Text}
            };
            LogParserSettings.IsConfigured = true;
            parser =new PlainTextLogFileLoader(LogParserSettings);
            return base.InitializeDataProviderAsync(logger);
        }

        public override Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
            => parser.Process(fileName, token, messagesHandler);
    }
}
