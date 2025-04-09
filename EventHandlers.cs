using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp330;
using Newtonsoft.Json;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using InventorySystem.Items.Usables.Scp330; // ✅ Добавено за Candy
using Exiled.API.Enums;
using PlayerStatsSystem;

namespace KillStatsTracker
{
    public class EventHandlers
    {
        private readonly string filePath = Path.Combine(Paths.Configs, "kills.json");

        public void OnPlayerDeath(DiedEventArgs ev)
        {
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] Player {ev.Player.Nickname} was killed by {ev.Attacker?.Nickname ?? "Unknown"}");

            if (ev.Attacker == null || ev.Attacker == ev.Player)
                return;

            string steamID = ev.Attacker.UserId;
            string name = ev.Attacker.Nickname;
            string cause = ev.DamageHandler.Type.ToString();

            var data = LoadData();
            if (!data.Players.ContainsKey(steamID))
            {
                if (KillStatsTracker.Instance?.Config.Debug == true)
                    Log.Info($"[DEBUG] Creating new player entry for {name} ({steamID})");
                data.Players[steamID] = new PlayerStats { Name = name, LastPlayed = DateTime.UtcNow };
            }

            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] {name} ({steamID}) killed with {cause}");

            if (ev.DamageHandler.Base is ExplosionDamageHandler explosion && explosion.ExplosionType == ExplosionType.Grenade)
            {
                data.Players[steamID].GrenadeKills++;
            }
            else if (cause.Contains("Scp"))
            {
                data.Players[steamID].SCPKills++;
            }
            else
            {
                data.Players[steamID].WeaponKills++;
            }

            data.Players[steamID].LastPlayed = DateTime.UtcNow;
            SaveData(data);
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] Data saved for {name} ({steamID})");
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            if (ev.Player.Role.Type != RoleTypeId.Scientist && ev.Player.Role.Type != RoleTypeId.ClassD)
                return;

            string steamID = ev.Player.UserId;
            string name = ev.Player.Nickname;

            var data = LoadData();
            if (!data.Players.ContainsKey(steamID))
            {
                data.Players[steamID] = new PlayerStats { Name = name, LastPlayed = DateTime.UtcNow };
            }

            data.Players[steamID].Escapes++;
            data.Players[steamID].LastPlayed = DateTime.UtcNow;

            SaveData(data);
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] {name} escaped! Total escapes: {data.Players[steamID].Escapes}");
        }

        public void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            if (ev.Candy != CandyKindID.Pink) return; // Проверяваме дали играчът е взел PinkCandy

            string steamID = ev.Player.UserId;
            string name = ev.Player.Nickname;

            var data = LoadData();
            if (!data.Players.ContainsKey(steamID))
            {
                data.Players[steamID] = new PlayerStats { Name = name, LastPlayed = DateTime.UtcNow };
            }

            data.Players[steamID].PinkCandyUsed++;
            data.Players[steamID].LastPlayed = DateTime.UtcNow;

            SaveData(data);
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] {name} picked up PinkCandy from SCP-330! Total picked up: {data.Players[steamID].PinkCandyUsed}");
        }

        public void OnCandyEaten(EatenScp330EventArgs ev)
        {
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] {ev.Player.Nickname} ate a candy: {ev.Candy.Kind}"); // ✅ Проверяваме какъв вид бонбон е изяден

            if (ev.Candy.Kind != CandyKindID.Pink) return; // ✅ Проверяваме дали бонбонът е PinkCandy

            string steamID = ev.Player.UserId;
            string name = ev.Player.Nickname;

            var data = LoadData();
            if (!data.Players.ContainsKey(steamID))
            {
                data.Players[steamID] = new PlayerStats { Name = name, LastPlayed = DateTime.UtcNow };
            }

            data.Players[steamID].PinkCandyUsed++;
            data.Players[steamID].LastPlayed = DateTime.UtcNow;

            SaveData(data);
            if (KillStatsTracker.Instance?.Config.Debug == true)
                Log.Info($"[DEBUG] {name} ate PinkCandy! Total used: {data.Players[steamID].PinkCandyUsed}");
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            var data = LoadData();
            data.RemoveInactivePlayers(KillStatsTracker.Instance!.Config.DeleteAfterDays);
            SaveData(data);

            if (KillStatsTracker.Instance!.Config.Webhook.Enabled)
            {
                WebhookSender.SendLeaderboard(data, KillStatsTracker.Instance!.Config);
            }
        }

        private PlayerData LoadData()
        {
            if (!File.Exists(filePath))
            {
                Log.Warn("[DEBUG] kills.json does not exist! Creating a new one...");
                SaveData(new PlayerData());
                return new PlayerData();
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (KillStatsTracker.Instance?.Config.Debug == true)
                    Log.Info($"[DEBUG] Loaded kills.json with {json.Length} characters.");
                return JsonConvert.DeserializeObject<PlayerData>(json) ?? new PlayerData();
            }
            catch (Exception ex)
            {
                Log.Error($"[ERROR] Failed to read kills.json: {ex.Message}");
                return new PlayerData();
            }
        }

        private void SaveData(PlayerData data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                if (KillStatsTracker.Instance?.Config.Debug == true)
                    Log.Info("[DEBUG] kills.json has been saved successfully!");
            }
            catch (Exception ex)
            {
                Log.Error($"[ERROR] Failed to save kills.json: {ex.Message}");
            }
        }
    }
}