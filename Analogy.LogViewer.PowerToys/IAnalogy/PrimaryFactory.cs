using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.LogViewer.PowerToys.Properties;

namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class PrimaryFactory : Analogy.LogViewer.Template.PrimaryFactory
    {
        internal static Guid Id { get; }= new Guid("e8c2c677-fec1-434a-a9b8-81540b79573a");

        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Affirmations";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Log Parser for Analogy Log Viewer";//override this
        public override Image SmallImage { get; set; } = Resources.Affirmations16x16;
        public override Image LargeImage { get; set; } = Resources.Affirmations32x32;


    }
}
