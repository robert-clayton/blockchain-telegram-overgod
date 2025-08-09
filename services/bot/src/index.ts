import { Hono } from 'hono'
import { env } from 'node:process'
import { fetch as undiciFetch } from 'undici'

// Placeholder import; wire the real SDK when configured
// import { SpacetimeClient } from '@spacetimedb/typescript-sdk'

const BOT_TOKEN = env.BOT_TOKEN || ''
const STDB_URL = env.STDB_URL || ''
const STDB_MODULE_ID = env.STDB_MODULE_ID || ''
const STDB_TOKEN = env.STDB_TOKEN || ''
const DEFAULT_SEASON_ID = Number(env.SEASON_ID || 1)

if (!BOT_TOKEN) {
  // eslint-disable-next-line no-console
  console.warn('BOT_TOKEN is not set; /webhook will not be able to reply to Telegram.')
}

// const stdb = new SpacetimeClient(STDB_URL, STDB_MODULE_ID, { token: STDB_TOKEN })
// await stdb.connect()

async function sendMessage(payload: { chat_id: number | string; text: string }) {
  if (!BOT_TOKEN) return
  await undiciFetch(`https://api.telegram.org/bot${BOT_TOKEN}/sendMessage`, {
    method: 'POST',
    headers: { 'content-type': 'application/json' },
    body: JSON.stringify(payload)
  })
}

const app = new Hono()

app.get('/', c => c.text('overgod-bot ok'))

app.post('/webhook', async c => {
  const update = await c.req.json().catch(() => undefined)
  const msg = update?.message
  const text: string | undefined = msg?.text
  const chatId = msg?.chat?.id

  if (!text || !chatId) return c.text('OK')

  if (text.startsWith('/leaderboards')) {
    const seasonId = DEFAULT_SEASON_ID
    // const top = await stdb.call('top_n', [seasonId, 10])
    // const lines = (top as any[]).map((r, i) => `${i + 1}. ${r[0]} — ${r[1]}`)
    const lines = ['(stub) 1. Alice — 12345', '(stub) 2. Bob — 11111']
    const body = lines.join('\n') || 'No scores yet.'
    await sendMessage({ chat_id: chatId, text: `Top 10 (S${seasonId})\n${body}` })
  }

  return c.text('OK')
})

export default app


