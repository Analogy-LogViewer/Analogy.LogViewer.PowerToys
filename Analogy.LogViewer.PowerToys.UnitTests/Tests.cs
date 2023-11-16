using Analogy.LogViewer.PowerToys.IAnalogy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.PowerToys.UnitTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var parser = new OfflineDataProvider();
            await parser.InitializeDataProvider(null);
            var results = await parser.Process("2020-10-03.txt", new CancellationToken(), new MessageHandlerForTesting());
            Assert.IsTrue(results.Count() == 61);
        }
    }
}