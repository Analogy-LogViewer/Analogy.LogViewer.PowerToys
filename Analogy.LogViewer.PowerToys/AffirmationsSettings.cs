namespace Analogy.LogViewer.PowerToys
{
    public class AffirmationsSettings
    {
        public int CheckInterval { get; set; }
        public string Address { get; set; }

        public AffirmationsSettings()
        {
            CheckInterval = 3000;
            Address = "https://www.affirmations.dev/";
        }
    }
}
