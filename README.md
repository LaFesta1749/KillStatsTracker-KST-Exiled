# KillStatsTracker

**KillStatsTracker** is an Exiled plugin for SCP:Secret Laboratory that tracks various player stats during gameplay and sends a summary via Discord webhook at the end of each round.

## 📊 Features
- Tracks:
  - 🔫 Weapon kills
  - 💣 Grenade kills
  - 🦠 SCP kills
  - 🍬 Pink Candy pickups (from SCP-330)
  - 🚪 Escapes (Class-D and Scientists)
- Sends a leaderboard summary via webhook at the end of each round
- Stores player stats in `kills.json`
- Automatically removes inactive players after a configurable number of days
- Configurable number of top players shown
- Optional debug mode with detailed logging

## ⚙ Configuration (config.yml)
```yaml
is_enabled: true
debug: false
top_players: 10
delete_after_days: 3
webhook:
  enabled: true
  url: "https://discord.com/api/webhooks/XXXXXX"
  footer: "SCP PARLAMATA"
  fields:
    show_weapon_kills: true
    show_grenade_kills: true
    show_scp_kills: true
    show_pink_candy_used: true
    show_escapes: true
```

## 🛠 Installation
1. Download the latest `.dll` release.
2. Place it in your server's `Exiled/Plugins` folder.
3. Restart the server.

## 👨‍💻 Author
Plugin by **LaFesta1749**

---

*This plugin is designed for use with Exiled 9.5.1 and SCP:Secret Laboratory.*