﻿using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.PowerToys.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class OfflineDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override Image? SmallImage { get; set; }
        public override Image? LargeImage { get; set; }
        public override string? OptionalTitle { get; set; } = "PowerToys Parser";
        public override string FileOpenDialogFilters { get; set; } = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        public override IEnumerable<string> SupportFormats { get; set; } = new List<string> { "*.txt" };
        public override string? InitialFolderFullPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\PowerToys\PowerToys Run\Logs");
        public override Guid Id { get; set; } = new Guid("e8bfbefe-912f-41d2-9faf-2f370d3695bc");
        private PlainTextLogFileLoader parser;
        private SplitterLogParserSettings LogParserSettings { get; set; }

        public OfflineDataProvider()
        {
            LogParserSettings = new SplitterLogParserSettings
            {
                Splitter = "|",
                SupportedFilesExtensions = new List<string> { "*.txt"},
                Maps = new Dictionary<int, AnalogyLogMessagePropertyName>
                {
                    { 0, AnalogyLogMessagePropertyName.Date},
                    { 1, AnalogyLogMessagePropertyName.Level},
                    { 2, AnalogyLogMessagePropertyName.Source},
                    { 3, AnalogyLogMessagePropertyName.Text},
                },
                IsConfigured = true,
            };
            parser = new PlainTextLogFileLoader(LogParserSettings);
        }
        public override Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
            => parser.Process(fileName, token, messagesHandler);
    }
}