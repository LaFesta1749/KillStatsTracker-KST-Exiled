using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Exiled.API.Features;

namespace KillStatsTracker
{
    public static class WebhookSender
    {
        private static readonly HttpClient HttpClient = new();

        public static void SendLeaderboard(PlayerData data, Config config)
        {
            if (!config.Webhook.Enabled || string.IsNullOrEmpty(config.Webhook.URL))
                return;

            var sorted = data.Players.Values.OrderByDescending(p => p.WeaponKills + p.GrenadeKills + p.SCPKills + p.Escapes + p.PinkCandyUsed).ToList();

            void SendEmbed(string title, string icon, List<(string name, int value)> entries)
            {
                if (entries.Count == 0) return;

                var embedPayload = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = $"🏆 {title} Leaderboard",
                            description = $"Top players with the most {title.ToLower()}.\n\n" +
                                          $"Top Players\n" +
                                          string.Join("\n", entries.Select((e, i) => $"{e.name} - {e.value}")),
                            color = 0xFF0000,
                            footer = new { text = "SCP PARLAMATA" }
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(embedPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient.PostAsync(config.Webhook.URL, content).GetAwaiter().GetResult();

                if (config.Debug)
                    Log.Info($"[DEBUG] Webhook sent for {title}");
            }

            if (config.Webhook.Fields.WeaponKills)
            {
                var top = sorted.Where(p => p.WeaponKills > 0)
                                .OrderByDescending(p => p.WeaponKills)
                                .Take(config.TopPlayers)
                                .Select(p => (p.Name, p.WeaponKills))
                                .ToList();
                SendEmbed("🔫 Weapon Kills", "🔫", top);
            }

            if (config.Webhook.Fields.GrenadeKills)
            {
                var top = sorted.Where(p => p.GrenadeKills > 0)
                                .OrderByDescending(p => p.GrenadeKills)
                                .Take(config.TopPlayers)
                                .Select(p => (p.Name, p.GrenadeKills))
                                .ToList();
                SendEmbed("💣 Grenade Kills", "💣", top);
            }

            if (config.Webhook.Fields.SCPKills)
            {
                var top = sorted.Where(p => p.SCPKills > 0)
                                .OrderByDescending(p => p.SCPKills)
                                .Take(config.TopPlayers)
                                .Select(p => (p.Name, p.SCPKills))
                                .ToList();
                SendEmbed("🦠 SCP Kills", "👹", top);
            }

            if (config.Webhook.Fields.PinkCandyUsed)
            {
                var top = sorted.Where(p => p.PinkCandyUsed > 0)
                                .OrderByDescending(p => p.PinkCandyUsed)
                                .Take(config.TopPlayers)
                                .Select(p => (p.Name, p.PinkCandyUsed))
                                .ToList();
                SendEmbed("🍬 Pink Candy Used", "🍬", top);
            }

            if (config.Webhook.Fields.Escapes)
            {
                var top = sorted.Where(p => p.Escapes > 0)
                                .OrderByDescending(p => p.Escapes)
                                .Take(config.TopPlayers)
                                .Select(p => (p.Name, p.Escapes))
                                .ToList();
                SendEmbed("🚪 Escapes", "🚪", top);
            }
        }
    }
}
