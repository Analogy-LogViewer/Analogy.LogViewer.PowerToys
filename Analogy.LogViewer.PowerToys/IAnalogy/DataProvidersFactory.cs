using System;
using System.Collections.Generic;
using Analogy.Interfaces;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class DataProvidersFactory : LogViewer.Template.DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public override string Title { get; set; } = "Log Parsers";
        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> {new PowerToysRegexOfflineDataProvider(), new OfflineDataProvider() };
    }
}
