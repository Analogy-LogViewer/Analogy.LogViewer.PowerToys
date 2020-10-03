using System;
using System.IO;
using Analogy.LogViewer.Template.Managers;
using Newtonsoft.Json;

namespace Analogy.LogViewer.PowerToys.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public string AffirmationsFileSetting { get; private set; } = "AnalogyAffirmationsSettings.json";
        public AffirmationsSettings Settings { get; set; }


        public UserSettingsManager()
        {
            if (File.Exists(AffirmationsFileSetting))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    string data = File.ReadAllText(AffirmationsFileSetting);
                    Settings = JsonConvert.DeserializeObject<AffirmationsSettings>(data, settings);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogException("Error loading user setting file", ex, "Analogy Serilog Parser");
                    Settings = new AffirmationsSettings();

                }
            }
            else
            {
                Settings = new AffirmationsSettings();
            }

        }

        public void Save()
        {
            try
            {
                File.WriteAllText(AffirmationsFileSetting, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                LogManager.Instance.LogException("Error saving settings: " + e.Message, e, "Analogy Serilog Parser");
            }


        }
    }
}