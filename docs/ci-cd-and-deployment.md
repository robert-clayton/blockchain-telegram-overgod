## CI/CD & Deployment — OvergodIdle

### Pipeline
- Build WebGL with `game-ci/unity-builder@v4`
- Upload artifact
- Detect folder containing `index.html` and deploy to `gh-pages` with `peaceiris/actions-gh-pages@v4`

### Requirements
- Repository Settings → Actions → Workflow permissions → Read and write
- Secrets: `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`

### Environments
- Staging: feature flags enabled, test purchases; deploy from feature branches if desired
- Production: `master` pushes deploy

### Troubleshooting
- 403 pushing gh-pages: ensure token write permissions
- 404/no index: ensure correct publish_dir or use auto-detect (already configured)


