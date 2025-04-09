using Exiled.API.Features;
using System;

namespace KillStatsTracker
{
    public class KillStatsTracker : Plugin<Config>
    {
        public override string Name => "KillStatsTracker";
        public override string Author => "LaFesta1749";
        public override string Prefix => "KST";
        public override Version Version => new Version(1, 0, 0);

        private EventHandlers? eventHandlers;
        public static KillStatsTracker? Instance { get; private set; } // ✅ Поправено (nullable)

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Died += eventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Scp330.EatenScp330 += eventHandlers.OnCandyEaten;
            Exiled.Events.Handlers.Player.Escaping += eventHandlers.OnEscape;
            Exiled.Events.Handlers.Server.RoundEnded += eventHandlers.OnRoundEnd;
            Log.Info($"KillStatsTracker v{Version} by {Author} has been enabled!");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Died -= eventHandlers!.OnPlayerDeath;
            Exiled.Events.Handlers.Scp330.EatenScp330 -= eventHandlers.OnCandyEaten;
            Exiled.Events.Handlers.Player.Escaping -= eventHandlers.OnEscape;
            Exiled.Events.Handlers.Server.RoundEnded -= eventHandlers!.OnRoundEnd;
            eventHandlers = null;
            Instance = null;

            Log.Info("KillStatsTracker has been disabled!");
        }
    }
}