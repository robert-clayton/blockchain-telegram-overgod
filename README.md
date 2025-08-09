## blockchain-telegram-overgod

An opinionated starter repo for building and shipping a Telegram Mini App idle game (OvergodIdle) with on-chain features.

### What this repo gives you
- **Auto-deploy to GitHub Pages**: CI builds WebGL and deploys to `gh-pages` on pushes to `master`.
- **Unity template project**: `OvergodIdle/` with working WebGL setup and a small UI that shows the Telegram user name.
- **Telegram integration**: via `Telegram-Unity-Bridge` (YesTMABridge) for Mini App APIs.
- **Blockchain integration**: via Openfort Unity SDK.

### Live build
- Deployed game (GitHub Pages): `https://robert-clayton.github.io/blockchain-telegram-overgod/`

## Repo layout
- `OvergodIdle/`: Unity project (target Unity `6000.1.15f1`).
  - `Assets/_ThirdParty/Openfort/` (git submodule)
  - `Assets/_ThirdParty/Telegram-Unity-Bridge/` (git submodule)
- `.github/workflows/build-webgl.yml`: CI to build and deploy to Pages.

## Prerequisites
- Unity: `6000.1.15f1` (LTS stream 6.x)
- Git
- Some editor

## Getting started
1) Clone with submodules
```
git clone --recurse-submodules https://github.com/robert-clayton/blockchain-telegram-overgod.git
cd blockchain-telegram-overgod
# If you already cloned without submodules:
git submodule update --init --recursive
```

2) Open `OvergodIdle/` in Unity and switch to WebGL
- File → Build Profiles → Web → Switch Platform
- Player Settings → Resolution and Presentation → WebGL Template → select `OvergodTelegram`

3) Play locally (Editor)
- The Telegram bridge JS is WebGL-only, so the editor shows a placeholder username.

4) Build and run WebGL locally
- Build → WebGL. Serve the build via a local web server (avoid `file://`).
- To simulate Telegram user data locally, append a dev user param to the URL:
  - `?dev_user={"id":1,"first_name":"Dev","last_name":"User","username":"dev"}`
- The in-game banner should display: `Telegram user: Dev User`.

## Telegram Mini App integration
- Package: `yes.tma.bridge` (from `Assets/_ThirdParty/Telegram-Unity-Bridge/packages/YesTMABridge`).
- Bridge expects a global `window.TGMiniAppGameSDKInstance` (provided by our `OvergodTelegram` template). In Telegram, it maps fields from `window.Telegram.WebApp.initDataUnsafe.user`.
- Minimal UI script: `TelegramUserNameUI` reads `TGMiniAppGameSDKProvider.GetUserInfo()` and shows the name at runtime in WebGL.

### Create and wire a Telegram bot to your game
1) Create a bot with BotFather
   - Open `@BotFather` in Telegram
   - `/newbot` → set name and username
2) Configure Web App domain
   - `/setdomain` → choose your bot → set domain to your GitHub Pages origin
   - Example: `robert-clayton.github.io`
3) Add a menu button that opens the Web App (optional but recommended)
   - `/setmenubutton` → choose your bot → select Web App
   - URL: `https://robert-clayton.github.io/blockchain-telegram-overgod/`
   - Text: e.g., `Play OvergodIdle`
4) Deep links you can use
   - Open Web App directly: `https://t.me/<YourBot>?startapp=<optional_param>`
   - Open bot chat with a parameter: `https://t.me/<YourBot>?start=<param>`
   - In channel posts, add an inline keyboard button with a `web_app` or `url` pointing to your Pages URL
     - Example (send via Bot API `sendMessage`):
       ```json
       {
         "chat_id": "@forge",
         "text": "Play OvergodIdle now!",
         "reply_markup": {
           "inline_keyboard": [[
             { "text": "Play", "web_app": { "url": "https://robert-clayton.github.io/blockchain-telegram-overgod/" } }
           ]]
         }
       }
       ```

Notes:
- The bridge reads Telegram user data only when launched inside the Telegram Mini App environment or when `dev_user` is provided.
- For production, ensure your Pages URL (and any CDN) is set in BotFather domain settings.

## Openfort blockchain support
- Package: `com.openfort.sdk` (from `Assets/_ThirdParty/Openfort/src/Packages/OpenfortSDK`).
- Dependencies handled via UPM:
  - `com.unity.nuget.newtonsoft-json: 3.2.0`
  - `com.cysharp.unitask: 2.3.3` (via OpenUPM registry)
- Explore the included sample: `Assets/_ThirdParty/Openfort/sample/` for login and wallet flows.
- Official docs: `https://docs.openfort.xyz/`

## Unity package setup (already configured)
- `OvergodIdle/Packages/manifest.json` includes:
  - Local packages:
    - `com.openfort.sdk`: `file:../Assets/_ThirdParty/Openfort/src/Packages/OpenfortSDK`
    - `yes.tma.bridge`: `file:../Assets/_ThirdParty/Telegram-Unity-Bridge/packages/YesTMABridge`
  - Scoped registry for OpenUPM (`com.cysharp`) used by UniTask.

## CI/CD: Build and deploy to GitHub Pages
- Workflow: `.github/workflows/build-webgl.yml`
  - Builds WebGL using `game-ci/unity-builder@v4` on Ubuntu.
  - Publishes artifact and deploys to `gh-pages` via `peaceiris/actions-gh-pages@v4`.
- Required repository configuration
  - Settings → Actions → General → Workflow permissions → enable “Read and write permissions”.
  - Settings → Pages → set source to `gh-pages` (root), or “GitHub Actions” depending on preference.
- Required secrets for Unity build
  - `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`

## Development notes
- Editor vs WebGL: JS bridge calls are guarded to avoid `EntryPointNotFoundException` outside WebGL.
- Local dev: Use the `dev_user` URL param while serving the build via a web server.

## License
- See `LICENSE`.
