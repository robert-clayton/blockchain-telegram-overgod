import LoadAssets from '@/components/LoadAssets';
import { AnimatePresence } from 'framer-motion';
import { Unity, useUnityContext } from 'react-unity-webgl';
import { TGMiniAppGameClientSDK } from '@open-yes/game-client-sdk';

declare global {
  interface Window {
    TGMiniAppGameSDKInstance: TGMiniAppGameClientSDK;
  }
}

window.TGMiniAppGameSDKInstance = new TGMiniAppGameClientSDK({
  projectId: 'overgod-idle',
  ui: {
    manifestUrl: new URL('./tonconnect-manifest.json', window.location.href).toString(),
    actionsConfiguration: {
      twaReturnUrl: 'https://t.me/OvergodIdle_bot',
    },
  },
});

const BASE_URL = (import.meta as any).env?.VITE_UNITY_BASE_URL ?? '.';

const App = () => {
  const { unityProvider, loadingProgression, isLoaded } = useUnityContext({
    loaderUrl: `${BASE_URL}/Build/dist.loader.js`,
    dataUrl: `${BASE_URL}/Build/dist.data`,
    frameworkUrl: `${BASE_URL}/Build/dist.framework.js`,
    codeUrl: `${BASE_URL}/Build/dist.wasm`,
    companyName: '',
    productName: 'Overgod Idle',
    productVersion: '0.1',
  });

  const progress = Math.round(loadingProgression * 100);
  return (
    <main className="w-screen h-screen of-hidden">
      <AnimatePresence>{!isLoaded && <LoadAssets progress={progress} />}</AnimatePresence>
      <Unity unityProvider={unityProvider} className="w-screen h-screen" />
    </main>
  );
};

export default App;
