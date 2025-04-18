using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Exiled.API.Features;
using KillStatsTracker; // ✅ Добавено, за да използваме PlayerData

namespace KillStatsTracker
{
    public static class WebhookSender
    {
        private static readonly HttpClient HttpClient = new();

        public static async void SendLeaderboard(PlayerData data, Config config)
        {
            if (!config.Webhook.Enabled || string.IsNullOrEmpty(config.Webhook.URL))
                return;
            if (config.Debug == true)
                Log.Info("[DEBUG] Sending leaderboard webhook...");
            if (config.Debug == true)
                Log.Info($"[DEBUG] Total players in JSON: {data.Players.Count}");

            var embedFields = new StringBuilder();
            int count = 0;

            foreach (var player in data.Players)
            {
                if (count >= config.TopPlayers) break;
                var stats = player.Value;
                embedFields.AppendLine($"**{stats.Name}**");
                if (config.Webhook.Fields.WeaponKills)
                    embedFields.AppendLine($"🔫 Weapon Kills: {stats.WeaponKills}");
                if (config.Webhook.Fields.GrenadeKills)
                    embedFields.AppendLine($"💣 Grenade Kills: {stats.GrenadeKills}");
                if (config.Webhook.Fields.SCPKills)
                    embedFields.AppendLine($"🦠 SCP Kills: {stats.SCPKills}");
                if (config.Webhook.Fields.PinkCandyUsed)
                    embedFields.AppendLine($"🍬 PinkCandy Used: {stats.PinkCandyUsed}");
                if (config.Webhook.Fields.Escapes)
                    embedFields.AppendLine($"🚪 Escapes: {stats.Escapes}");
                embedFields.AppendLine();
                count++;
            }

            if (config.Debug == true)
                Log.Info($"[DEBUG] Players in leaderboard: {count}");

            var json = new
            {
                embeds = new[]
                {
                    new
                    {
                        title = "🏆 Kill Leaderboard 🏆",
                        description = embedFields.ToString(),
                        color = 16711680,
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

            try
            {
                await HttpClient.PostAsync(config.Webhook.URL, content);
                if (config.Debug == true)
                    Log.Info("[DEBUG] Webhook sent successfully!");
            }
            catch (Exception ex)
            {
                if (config.Debug == true)
                    Log.Error($"[ERROR] Failed to send webhook: {ex.Message}");
            }
        }
    }
}