# Overgod Telegram Bot Service

Small webhook service that handles bot commands (e.g., `/leaderboards`) and queries SpacetimeDB for results.

## Env

- BOT_TOKEN: Telegram Bot API token
- STDB_URL: SpacetimeDB endpoint
- STDB_MODULE_ID: Deployed module id/name
- STDB_TOKEN: Auth token for the module (if required)
- SEASON_ID: Default season id (number)

## Run locally

- Use any Node runtime. Example with tsx:
  - npm i
  - npm run dev
- Expose a public URL (ngrok/cloudflared) and set webhook:

```
curl -X POST "https://api.telegram.org/bot$BOT_TOKEN/setWebhook" \
  -d url="https://<your-public-url>/webhook" \
  -d secret_token="<random>"
```

## Deploy

- Deploy to your platform of choice (Cloudflare Workers, Fly.io, Render, etc.).
- Set the same env vars in your hosting provider.
- Re-run `setWebhook` with the deployed URL.

## SpacetimeDB

- Replace the stubbed calls in `src/index.ts` with real SDK usage once your module is deployed.
- Expected procedures:
  - `top_n(season_id: number, n: number) -> Array<[username: string, score: number]>`


