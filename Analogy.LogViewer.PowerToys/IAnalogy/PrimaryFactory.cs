using Analogy.Interfaces;
using Analogy.LogViewer.PowerToys.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class PrimaryFactory : Analogy.LogViewer.Template.PrimaryFactory
    {
        internal static Guid Id { get; }= new Guid("9ba595ac-e1a1-4a97-819a-4f42a9518d78");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "PowerToys";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Analogy Log Parser for Microsoft PowerToys";
        public override Image? SmallImage { get; set; } = Resources.powertoys16x16;
        public override Image? LargeImage { get; set; } = Resources.powertoys32x32;
    }
}