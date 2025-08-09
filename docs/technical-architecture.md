## Technical Architecture — OvergodIdle

### Client
- Unity WebGL (6000.1.15f1)
- Telegram bridge: `YesTMABridge` (`yes.tma.bridge`)
  - JS interop via `.jslib`; expects `window.TGMiniAppGameSDKInstance`
  - Template `Assets/WebGLTemplates/OvergodTelegram` provides instance (maps Telegram WebApp user)
- Blockchain: Openfort Unity SDK (`com.openfort.sdk`)
  - Custodial wallet; optional TON/Stars payments (Stars via Telegram, TON via Openfort integration)
- State persistence
  - Local: PlayerPrefs/localStorage for cache
  - Remote (minimal): profile, currency balances, leaderboard submissions
- Feature flags and configs via JSON pushed with builds

### Backend (minimal-first)
- Option A (initial): GitHub Pages static hosting + lightweight cloud worker
  - Cloudflare Workers / Firebase Functions for: leaderboards, signed stamps, telemetry intake
- Data model
  - Player: id (Telegram user id), profile, progression summary
  - Leaderboard: season, metric, rank
  - Telemetry: event stream (anonymized)

### Security/Integrity
- Server time reference for offline gain clamps
- HMAC for sensitive requests; nonce + short TTL
- No private keys on client; Openfort handles custody

### Build & Deploy
- GitHub Actions builds WebGL and deploys to `gh-pages`
- Template selection: `OvergodTelegram`
- Environment
  - Staging vs prod via build param and template flag; staging can enable test purchases

### Observability
- Client event queue → POST to worker endpoint
- Dashboards: retention, funnels, monetization, economy

### Dependencies
- Unity UPM packages per `manifest.json` (including OpenUPM for UniTask)


