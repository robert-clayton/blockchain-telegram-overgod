## SpacetimeDB Backend — OvergodIdle

This project uses a SpacetimeDB module to host game state and simple server logic (leaderboards, referrals, stamps).

### Layout

- `backend/spacetimedb/overgod/`: Rust crate compiled to Wasm and deployed to SpacetimeDB.
- `services/bot/`: Telegram bot webhook that queries SpacetimeDB.

### Development

1) Install SpacetimeDB CLI and toolchain (see official docs).
2) Implement tables and procedures in `src/lib.rs`.
3) Build and deploy to your SpacetimeDB instance (Maincloud or self-hosted).
4) Configure the bot with `STDB_URL`, `STDB_MODULE_ID`, `STDB_TOKEN` and set Telegram webhook.

### Expected Procedures

- `post_score(season_id: i32, user_id: i64, username: String, score: i64)`
- `top_n(season_id: i32, n: i32) -> Vec<(String, i64)>`

### Notes

- Keep Unity client read-only with respect to authoritative state; write via server-side procedures.
- Use short-lived signatures/nonces for sensitive writes.


