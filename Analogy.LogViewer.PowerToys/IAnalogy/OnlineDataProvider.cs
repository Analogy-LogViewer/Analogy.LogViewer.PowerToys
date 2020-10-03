using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.PowerToys.Managers;
using Analogy.LogViewer.PowerToys.Properties;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;
namespace Analogy.LogViewer.PowerToys.IAnalogy
{
    public class OnlineDataProvider : Analogy.LogViewer.Template.OnlineDataProvider
    {
        private Timer OnlineFetcher { get; set; }
        private HttpClient httpClient { get; set; }
        public override Guid Id { get; set; } = new Guid("9cb40d24-c91b-4eff-b641-b1c6f9503a60");
        public override string OptionalTitle { get; set; } = "Affirmations";
        public override async Task<bool> CanStartReceiving() => await Task.FromResult(true);
        public override Image ConnectedLargeImage { get; set; } = Resources.Affirmations32x32;
        public override Image ConnectedSmallImage { get; set; } = Resources.Affirmations16x16;
        public override Image DisconnectedLargeImage { get; set; } = Resources.Affirmations32x32;
        public override Image DisconnectedSmallImage { get; set; } = Resources.Affirmations16x16;
      
        public override async Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            await base.InitializeDataProviderAsync(logger);
            httpClient = new HttpClient { BaseAddress = new Uri(UserSettingsManager.UserSettings.Settings.Address) };
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Analogy Affirmations");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            OnlineFetcher = new Timer(UserSettingsManager.UserSettings.Settings.CheckInterval);

            OnlineFetcher.Elapsed += async (s, e) =>
            {
                HttpResponseMessage response = await httpClient.GetAsync("/");
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                AffirmationData affirmation = JsonConvert.DeserializeObject<AffirmationData>(resp);
                AnalogyLogMessage m = new AnalogyInformationMessage(affirmation.affirmation, UserSettingsManager.UserSettings.Settings.Address);
                MessageReady(this, new AnalogyLogMessageArgs(m, Environment.MachineName, "Example", Id));
            };

        }
        public override Task StartReceiving()
        {
            OnlineFetcher?.Start();
            return Task.CompletedTask;
        }

        public override Task StopReceiving()
        {
            OnlineFetcher?.Stop();
            return Task.CompletedTask;
        }

    }


    public class AffirmationData
    {
        public string affirmation { get; set; }
    }

}

