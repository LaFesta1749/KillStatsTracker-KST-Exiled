using Exiled.API.Interfaces;
using System.ComponentModel;

namespace KillStatsTracker
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        public int DeleteAfterDays { get; set; } = 3;
        public int TopPlayers { get; set; } = 5;

        public WebhookConfig Webhook { get; set; } = new();
        public WeeklyResetConfig WeeklyReset { get; set; } = new();
    }

    public class WebhookConfig
    {
        public bool Enabled { get; set; } = true;
        public string URL { get; set; } = "YOUR_DISCORD_WEBHOOK";

        public WebhookFields Fields { get; set; } = new();
    }

    public class WebhookFields
    {
        public bool WeaponKills { get; set; } = true;
        public bool GrenadeKills { get; set; } = true;
        public bool SCPKills { get; set; } = true;
        public bool Escapes { get; set; } = true;
        public bool PinkCandyUsed { get; set; } = true;
    }

    public class WeeklyResetConfig
    {
        public bool Enabled { get; set; } = true;
        public DayOfWeek Day { get; set; } = DayOfWeek.Sunday;
        public int Hour { get; set; } = 23;
        public int Minute { get; set; } = 59;
        public int Second { get; set; } = 59;
    }
}