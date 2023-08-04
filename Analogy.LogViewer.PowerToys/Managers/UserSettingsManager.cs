using System;
using System.IO;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Analogy.LogViewer.PowerToys.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public string PowerToysSettingsFileSetting { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", "AnalogyPowerToysSettings.json");
        public PowerToysSettings Settings { get; set; }


        public UserSettingsManager()
        {
            if (File.Exists(PowerToysSettingsFileSetting))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    string data = File.ReadAllText(PowerToysSettingsFileSetting);
                    Settings = JsonConvert.DeserializeObject<PowerToysSettings>(data, settings)!;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogError(ex, "Error loading user setting file", ex, "Analogy Power Toys Settings");
                    Settings = new PowerToysSettings();

                }
            }
            else
            {
                Settings = new PowerToysSettings();
            }

        }

        public void Save()
        {
            try
            {
                File.WriteAllText(PowerToysSettingsFileSetting, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                LogManager.Instance.LogError(e, "Error saving settings: " + e.Message, e, "Analogy Power Toys Settings");
            }


        }
    }
}