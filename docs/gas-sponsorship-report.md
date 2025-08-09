## Gas Sponsorship Report — OvergodIdle

This report explains the ramifications of sponsoring blockchain gas fees for players, estimates COGS in USD terms under different usage scenarios, and outlines best practices, trade-offs, and real-world patterns from existing web3 games.

### Executive summary
- Sponsoring gas removes a major UX barrier, increasing conversion and retention, especially during onboarding and time-limited events.
- Treat gas as a controllable COGS line item. With guardrails, it can be kept predictable and modest on modern low-cost EVM L2s.
- Recommended: sponsor narrowly (whitelisted methods), cap per-user, and batch operations. Use Stars for payments, and ApeChain (EVM) for asset custody with sponsored gas.

### What is gas sponsorship?
- The developer (or a relayer) pays the network fee on behalf of the player for specific contract calls.
- Implemented via relayers/meta-transactions and sponsor policies. In Openfort terms, you configure which methods/contracts are eligible and under what limits.

### Why sponsor gas? (UX and funnel impact)
- Removes wallet funding friction (no “buy token first”), improving:
  - First-action conversion (first mint/claim/upgrade),
  - Session completion rates for timed loops,
  - Viral mechanics (referrals/redeems) where you want zero-blockers.
- Real-world patterns (qualitative):
  - Gasless onboarding is common among successful titles using L2s or custom rollups (e.g., Immutable ecosystem titles like Gods Unchained; Skyweaver via Sequence gasless meta-txs; Reddit Avatars free mints on Polygon; Flow-based titles like NBA Top Shot avoiding end-user gas). The shared theme is: remove crypto funding steps at the start.

### Cost model (USD)
Let:
- gasPriceNative = current gas price in native token per gas unit (e.g., in gwei for ETH-like)
- gasUsed = intrinsic gas used by the tx (depends on method)
- tokenUSD = native token USD price

Then approximate tx cost in USD:

  costUSD ≈ gasPriceNative × gasUsed × tokenUSD

For EVM chains using gwei and wei:
- If gasPrice is in gwei and token is ETH-like, costUSD ≈ (gasPrice_gwei × 1e-9) × gasUsed × tokenUSD

Typical gasUsed (very rough, depends on contracts):
- Simple meta-transaction execution: ~30k–60k gas
- ERC-1155 single mint/transfer: ~50k–90k gas
- ERC-721 mint: ~70k–120k gas
- Batch mints (ERC-1155): amortize more efficiently per item

On modern low-cost L2s, you can often target ~$0.0005–$0.01 per sponsored tx, subject to gas markets and token USD price.

Scenario planning (illustrative):
- Onboarding-only sponsorship:
  - DAU 10,000; 1 sponsored tx/user/day; $0.002/tx → ~$20/day → ~$600/month
  - DAU 50,000; 1 tx/user/day; $0.002/tx → ~$100/day → ~$3,000/month
- Liveops event with more txs:
  - DAU 20,000; avg 3 sponsored txs/user/day; $0.0015/tx → ~$90/day → ~$2,700/month
- Power users capped at 10 sponsored txs/day to bound costs.

Key drivers:
- gasUsed variability by method, chain base fee, L1 data costs for rollups, native token price volatility, and your tx-per-user profile.

### Budgeting as COGS
- Start with a monthly cap (e.g., 5–10% of gross revenue target), then back-solve per-user caps.
- Maintain a “sponsor pool” funded from Stars revenue; pause or tighten caps if pool drains too fast.
- Track CAC/LTV: if sponsorship lifts conversion/retention sufficiently, it pays for itself.

### Best practices to control cost
1) Whitelist only key methods (e.g., first-claim, season reward, cosmetic equip) rather than all gameplay txs.
2) Per-user daily caps and rate limits (per Telegram id/IP/device). Hard stop + clear UI when exceeded.
3) Batch ops:
   - Use ERC-1155 batch mints for seasonal/cosmetic drops.
   - Support multicall where possible to reduce per-action overhead.
4) Lazy minting: sign off-chain and mint on first equip/transfer (or on claim windows), reducing total on-chain txs.
5) Time-shift settlement: queue and process during low-demand windows if chain economics differ intra-day.
6) Use cheaper chains/rollups for high-volume actions; reserve L1 bridges only for special redemptions.
7) Abuse protection: allowlist origins, require user signatures, and bind to Telegram identity. Block replays.

### Risks and mitigations
- Abuse/farming: set caps, require session signatures, and anomaly detection.
- Volatility: native token USD spikes → auto-adjust caps or temporarily disable non-essential sponsored methods.
- Outage/queue buildup: degrade gracefully (fall back to off-chain receipts and settle later).
- Compliance constraints in Mini Apps: keep payments inside Telegram rules (use Stars); keep on-chain user actions cosmetic/ownership-focused.

### Implementation notes (ApeChain with Openfort)
- Configure Openfort project to use ApeChain (EVM) RPC, chainId, explorer, currency.
- Define Sponsor Policy: contracts + methods eligible, per-user/day caps, global caps.
- Unity client:
  - Initialize Openfort SDK with ApeChain network.
  - Route eligible gameplay calls through the sponsor/relayer.
  - Display budget status in UI when approaching cap; offer alternatives (queue for later, or user-funded advanced action).

### Where to sponsor vs. not
- Sponsor: onboarding actions, seasonal reward claims, purely cosmetic mints/equips, community quests.
- Consider non-sponsored: high-frequency or optional power-user operations; allow users to self-fund or wait.

### Real-world patterns from games/products
- Immutable ecosystem (e.g., Gods Unchained) popularized gas-free user experiences via L2 infrastructure.
- Skyweaver (by Horizon/Sequence) used gasless meta-transactions for smooth onboarding.
- Reddit Collectible Avatars enabled free mints on Polygon at scale, avoiding user-paid gas.
- Flow-based titles (e.g., NBA Top Shot) hide gas from users entirely; akin to full sponsorship by the platform.

These examples show the same lesson: removing the funding step grows funnels. The operational challenge is keeping costs bounded via caps and smart contract design.

### Quick calculator (plug your numbers)
- Inputs:
  - DAU = 20,000
  - tx/user/day = 2
  - gasUsed = 60,000
  - gasPrice = 1.5 gwei (L2 illustrative)
  - tokenUSD = $2.00
- Per-tx costUSD ≈ (1.5e-9) × 60,000 × 2.00 ≈ $0.00018
- Daily cost ≈ 20,000 × 2 × $0.00018 ≈ $7.2
- Monthly (30d) ≈ ~$216 (before variance)

Always validate on your target chain by measuring live gasUsed for your exact methods and tracking native token prices.

### Action items for OvergodIdle
- Default to sponsoring: onboarding claim, season cosmetics equip, referral reward claim.
- Cap: 10 sponsored txs/day/user; global monthly budget; telemetry + alerts.
- Economy link: reserve 5–10% of monthly gross for sponsor pool; tune after first two seasons.
- Add a settings flag to switch sponsorship off per environment build.


