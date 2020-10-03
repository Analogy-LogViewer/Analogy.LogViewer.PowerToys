using System.Linq;
using Analogy.LogViewer.PowerToys.IAnalogy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.PowerToys.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var parser = new OfflineDataProvider();
            await parser.InitializeDataProviderAsync(null);
            var results = await parser.Process("2020-10-03.txt", new CancellationToken(), new MessageHandlerForTesting());
            Assert.IsTrue(results.Count()==60);


        }
    }
}
