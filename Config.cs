using Exiled.API.Interfaces;
using System.ComponentModel;

namespace KillStatsTracker
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Enable debug logs.")]
        public bool Debug { get; set; } = false;

        [Description("How many top players should be sent in the webhook.")]
        public int TopPlayers { get; set; } = 10;

        [Description("How many days before a player's data is deleted if inactive.")]
        public int DeleteAfterDays { get; set; } = 3;

        [Description("Webhook settings.")]
        public WebhookConfig Webhook { get; set; } = new WebhookConfig();
    }

    public class WebhookConfig
    {
        public bool Enabled { get; set; } = true;
        public string URL { get; set; } = "https://discord.com/api/webhooks/XXXXXX";
        public string Footer { get; set; } = "SCP PARLAMATA";

        public WebhookFields Fields { get; set; } = new WebhookFields();
    }

    public class WebhookFields
    {
        public bool ShowWeaponKills { get; set; } = true;
        public bool ShowGrenadeKills { get; set; } = true;
        public bool ShowSCPKills { get; set; } = true;
        public bool ShowPinkCandyUsed { get; set; } = true;
        public bool ShowEscapes { get; set; } = true;
    }
}