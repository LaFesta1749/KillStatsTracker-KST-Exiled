using System;
using System.Collections.Generic;
using System.Linq;

namespace KillStatsTracker
{
    public class PlayerData
    {
        public Dictionary<string, PlayerStats> Players { get; set; } = new();

        public void RemoveInactivePlayers(int days)
        {
            DateTime cutoff = DateTime.UtcNow.AddDays(-days);
            Players = Players
                .Where(p => p.Value.LastPlayed >= cutoff)
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }

    public class PlayerStats
    {
        public string Name { get; set; } = "Unknown";
        public int WeaponKills { get; set; }
        public int GrenadeKills { get; set; }
        public int SCPKills { get; set; }
        public int PinkCandyUsed { get; set; }
        public int Escapes { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}