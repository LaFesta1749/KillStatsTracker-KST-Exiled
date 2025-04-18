![Downloads](https://img.shields.io/github/downloads/LaFesta1749/KillStatsTracker-KST-Exiled/total?label=Downloads&style=for-the-badge)
[![Discord](https://img.shields.io/badge/Discord-Join-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/PTmUuxuDXQ)

# KillStatsTracker

An **Exiled plugin** for **SCP: Secret Laboratory** that tracks:

- 🔫 Weapon kills
- 💣 Grenade kills
- 👹 SCP kills
- 🍬 PinkCandy usage
- 🚪 Player escapes

...and sends a **separate leaderboard** for each to your **Discord Webhook** at the end of every round.

## 💻 Features
- 🏆 Multiple individual leaderboard embeds
- 📆 Optional **Weekly Reset System**
  - Resets stats every Sunday at 23:59:59 Bulgarian time (if enabled)
- 🗑️ Configurable cleanup of inactive players (after X days)
- ⚙️ Fully customizable via `config.yml`
- 🧪 Debug mode for console logs

## 📦 Requirements
- Exiled **9.5.1**
- A valid Discord Webhook

## 🛠️ Configuration
Located in `config.yml`:
```yml
TopPlayers: 5
DeleteAfterDays: 3
Debug: true

Webhook:
  Enabled: true
  URL: "https://discord.com/api/webhooks/..."
  Fields:
    WeaponKills: true
    GrenadeKills: true
    SCPKills: true
    Escapes: true
    PinkCandyUsed: true

WeeklyReset:
  Enabled: true
  Day: Sunday
  Hour: 23
  Minute: 59
  Second: 59
```

## 👤 Author
**LaFesta1749**