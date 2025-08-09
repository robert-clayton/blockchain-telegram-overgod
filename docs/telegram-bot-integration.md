## Telegram Bot Integration — OvergodIdle

### Bot Setup (BotFather)
1. `/newbot` → name + username
2. `/setdomain` → set to your Pages origin, e.g., `robert-clayton.github.io`
3. `/setmenubutton` → Web App → URL: `https://robert-clayton.github.io/blockchain-telegram-overgod/`
4. Optional: `/setuserpic`, `/setabouttext`, `/setdescription`

### Deep Links
- Open Web App directly: `https://t.me/<YourBot>?startapp=<param>`
- Open bot with param: `https://t.me/<YourBot>?start=<param>`
- Channel post inline button (web_app): see README example

### WebApp Data Usage
- Template maps `Telegram.WebApp.initDataUnsafe.user` → `TGMiniAppGameSDKInstance.launchParams.initData.user`
- C# reads via `TGMiniAppGameSDKProvider.GetUserInfo()`

### Referrals
- Generate signed referral params for `startapp`
- Credit referrer when invitee reaches milestones; reward cosmetics/Stardust trickles

### Admin Tools (bot commands)
- `/post_leaderboard` → snapshot top N and post to channel
- `/season_time` → reply with remaining time
- `/code <reward>` → generate limited-use cosmetic codes (optional)


