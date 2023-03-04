using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.PowerToys.Parser
{
    public class PlainTextLogFileLoader
    {
        private ISplitterLogParserSettings _logFileSettings;
        private PlainLogFileParser _parser;
        public PlainTextLogFileLoader(ISplitterLogParserSettings logFileSettings)
        {
            _logFileSettings = logFileSettings;
            _parser = new PlainLogFileParser(_logFileSettings);
        }
        public async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            if (!_logFileSettings.IsConfigured)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File Parser is not configured. Please set it first in the settings Window",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            if (!_logFileSettings.CanOpenFile(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File {fileName} Is not supported or not configured correctly in the windows settings",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            List<IAnalogyLogMessage> messages = new List<IAnalogyLogMessage>();
            try
            {
                long count = 0;
                AnalogyLogMessage? entry = null;
                using (var stream = File.OpenRead(fileName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            var items = line.Split(_parser.splitters, StringSplitOptions.None);

                            if (items.Length == 4 && DateTime.TryParse(items[0], out var dateTime))
                            {
                                if (entry != null)
                                {
                                    messages.Add(entry);
                                    entry = null;
                                    count++;
                                    messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                                }
                                entry = _parser.Parse(line);
                            }
                            else if (entry != null)
                            {
                                if (entry.Text == "")
                                {
                                    entry.Text = line;
                                }
                                else
                                {
                                    entry.Text += Environment.NewLine + line;
                                }
                            }
                        }
                    }

                    if (entry != null)
                    {
                        messages.Add(entry);

                    }
                }
                messagesHandler.AppendMessages(messages, fileName);
                return messages;
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error occured processing file {fileName}. Reason: {e.Message}",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
        }

        private static string GetFileNameAsDataSource(string fileName)
        {
            string file = Path.GetFileName(fileName);
            return fileName.Equals(file) ? fileName : $"{file} ({fileName})";
        }
    }
}
