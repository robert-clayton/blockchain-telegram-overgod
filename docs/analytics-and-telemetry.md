## Analytics & Telemetry — OvergodIdle

### Core KPIs
- Retention: D1, D7, D30
- Engagement: sessions/day, session length
- Monetization: ARPDAU, conversion, payer retention, attach rates
- Progression: ascensions/day, time to first ascension

### Event Taxonomy (initial)
- app_start, app_quit
- user_identified (telegram_id, username)
- resource_gain/loss (type, amount, source)
- upgrade_purchase (id, cost, new_level)
- expedition_start/complete (id, duration, rewards)
- ascension (points_earned, total_resets)
- store_spend (sku, price_usd, currency)
- referral_event (referrer_id, type)

### Pipeline
- Client batches events → POST to Worker endpoint
- Storage: object storage + daily parquet
- Dashboard: lightweight notebook/BI, or 3rd-party if allowed

### Privacy
- No PII beyond Telegram user id/username
- Honor deletion requests; simple opt-out flag


