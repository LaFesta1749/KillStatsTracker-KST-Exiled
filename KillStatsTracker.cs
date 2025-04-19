using Exiled.API.Features;
using System;
using System.IO;
using System.Timers;
using Timer = System.Timers.Timer;
using KillStatsTracker;

namespace KillStatsTracker
{
    public class KillStatsTracker : Plugin<Config>
    {
        public override string Name => "KillStatsTracker";
        public override string Author => "LaFesta1749";
        public override string Prefix => "KST";
        public override Version Version => new(1, 0, 3);
        public static KillStatsTracker? Instance { get; private set; }

        private EventHandlers? eventHandlers;
        private Timer? weeklyResetTimer;

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Died += eventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Scp330.EatenScp330 += eventHandlers.OnCandyEaten;
            Exiled.Events.Handlers.Player.Escaping += eventHandlers.OnEscape;
            Exiled.Events.Handlers.Server.RoundEnded += eventHandlers.OnRoundEnd;

            SetupWeeklyReset();
            Log.Info($"KillStatsTracker v{Version} by {Author} has been enabled!");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Died -= eventHandlers!.OnPlayerDeath;
            Exiled.Events.Handlers.Scp330.EatenScp330 -= eventHandlers.OnCandyEaten;
            Exiled.Events.Handlers.Player.Escaping -= eventHandlers.OnEscape;
            Exiled.Events.Handlers.Server.RoundEnded -= eventHandlers.OnRoundEnd;

            eventHandlers = null;
            Instance = null;

            weeklyResetTimer?.Stop();
            weeklyResetTimer?.Dispose();
            weeklyResetTimer = null;

            Log.Info("KillStatsTracker has been disabled!");
        }

        private void SetupWeeklyReset()
        {
            if (!Config.WeeklyReset.Enabled)
                return;

            try
            {
                TimeZoneInfo bgTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Sofia"); // Bulgarian time
                DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, bgTimeZone);

                int daysUntilReset = ((int)Config.WeeklyReset.Day - (int)now.DayOfWeek + 7) % 7;
                DateTime nextReset = now.Date.AddDays(daysUntilReset)
                    .AddHours(Config.WeeklyReset.Hour)
                    .AddMinutes(Config.WeeklyReset.Minute)
                    .AddSeconds(Config.WeeklyReset.Second);

                double timeUntilReset = (nextReset - now).TotalMilliseconds;
                if (timeUntilReset < 0)
                    timeUntilReset += TimeSpan.FromDays(7).TotalMilliseconds;

                weeklyResetTimer = new Timer(timeUntilReset)
                {
                    AutoReset = false
                };
                weeklyResetTimer.Elapsed += (_, _) => ResetWeeklyStats();
                weeklyResetTimer.Start();

                if (Config.Debug)
                    Log.Info($"[DEBUG] Weekly reset scheduled for: {nextReset} (BG time)");
            }
            catch (Exception ex)
            {
                Log.Error($"[ERROR] Failed to schedule weekly reset: {ex.Message}");
            }
        }

        private void ResetWeeklyStats()
        {
            try
            {
                string path = Path.Combine(Paths.Configs, "kills.json");

                if (File.Exists(path))
                    File.Delete(path);

                File.WriteAllText(path, "{\n  \"Players\": {}\n}");

                if (Config.Debug)
                    Log.Info("[KillStatsTracker] Weekly reset completed!");
            }
            catch (Exception ex)
            {
                Log.Error($"[ERROR] Weekly reset failed: {ex.Message}");
            }

            SetupWeeklyReset();
        }
    }
}
